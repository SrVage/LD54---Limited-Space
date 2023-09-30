
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Hash128 = Unity.Entities.Hash128;

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Rukhanka
{

[DisableAutoCreation]
[RequireMatchingQueriesForUpdate]
public partial struct AnimationProcessSystem: ISystem
{
	EntityQuery
		animatedObjectQuery,
		boneEntityQuery;

	NativeParallelHashMap<Hash128, BlobAssetReference<BoneRemapTableBlob>> rigToSkinnedMeshRemapTables;
	NativeList<int2> bonePosesOffsetsArr;

	BufferTypeHandle<AnimationToProcessComponent> animationToProcessBufHandle;
	BufferTypeHandle<AnimatorControllerParameterComponent> animatorParameterBufHandleRW;

	ComponentTypeHandle<RigDefinitionComponent> rigDefinitionTypeHandle;
	ComponentLookup<RigDefinitionComponent> rigDefinitionLookup;
	ComponentLookup<LocalTransform> localTransfromLookup;
	ComponentLookup<Parent> parentComponentLookup;
	ComponentTypeHandle<AnimatorControllerParameterIndexTableComponent> animatorParameterIndexTableHandle;

	BufferLookup<AnimationToProcessComponent> animationToProcessBufferLookup;
	BufferLookup<RootMotionAnimationStateComponent> rootMotionAnimationStateBufferLookupRW;

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[BurstCompile]
	public void OnCreate(ref SystemState ss)
	{
		InitializeRuntimeData(ref ss);

		bonePosesOffsetsArr = new (Allocator.Persistent);

		using var eqb0 = new EntityQueryBuilder(Allocator.Temp)
		.WithAll<RigDefinitionComponent, AnimationToProcessComponent>();
		animatedObjectQuery = ss.GetEntityQuery(eqb0);

		using var eqb3 = new EntityQueryBuilder(Allocator.Temp)
        .WithAll<LocalTransform, AnimatorEntityRefComponent>();
		boneEntityQuery = ss.GetEntityQuery(eqb3);

		animationToProcessBufHandle = ss.GetBufferTypeHandle<AnimationToProcessComponent>(true);
		animatorParameterBufHandleRW = ss.GetBufferTypeHandle<AnimatorControllerParameterComponent>();
		rigDefinitionTypeHandle = ss.GetComponentTypeHandle<RigDefinitionComponent>(true);
		rigDefinitionLookup = ss.GetComponentLookup<RigDefinitionComponent>(true);
		parentComponentLookup = ss.GetComponentLookup<Parent>(true);
		localTransfromLookup = ss.GetComponentLookup<LocalTransform>(true);
		animatorParameterIndexTableHandle = ss.GetComponentTypeHandle<AnimatorControllerParameterIndexTableComponent>();
		animationToProcessBufferLookup = ss.GetBufferLookup<AnimationToProcessComponent>(true);
		rootMotionAnimationStateBufferLookupRW = ss.GetBufferLookup<RootMotionAnimationStateComponent>();
	}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[BurstCompile]
	public void OnDestroy(ref SystemState ss)
	{
		if (rigToSkinnedMeshRemapTables.IsCreated)
			rigToSkinnedMeshRemapTables.Dispose();

		if (bonePosesOffsetsArr.IsCreated)
			bonePosesOffsetsArr.Dispose();

		if (SystemAPI.TryGetSingleton<RuntimeAnimationData>(out var rad))
		{
			rad.Dispose();
			ss.EntityManager.DestroyEntity(SystemAPI.GetSingletonEntity<RuntimeAnimationData>());
		}
	}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void InitializeRuntimeData(ref SystemState ss)
	{
		var rad = RuntimeAnimationData.MakeDefault();
		ss.EntityManager.CreateSingleton(rad, "Rukhanka.RuntimeAnimationData");
	}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	JobHandle PrepareComputationData(ref SystemState ss, NativeArray<int> chunkBaseEntityIndices, ref RuntimeAnimationData runtimeData, NativeList<Entity> entitiesArr, JobHandle dependsOn)
	{
		rigDefinitionTypeHandle.Update(ref ss);
		
		//	Calculate bone offsets per entity
		var calcBoneOffsetsJob = new CalculateBoneOffsetsJob()
		{
			chunkBaseEntityIndices = chunkBaseEntityIndices,
			bonePosesOffsets = bonePosesOffsetsArr.AsArray(),
			rigDefinitionTypeHandle = rigDefinitionTypeHandle
		};

		var jh = calcBoneOffsetsJob.ScheduleParallel(animatedObjectQuery, dependsOn);

		//	Do prefix sum to calculate absolute offsets
		var prefixSumJob = new DoPrefixSumJob()
		{
			boneOffsets = bonePosesOffsetsArr.AsArray()
		};

		prefixSumJob.Schedule(jh).Complete();

		var boneBufferLen = bonePosesOffsetsArr[^1];
		runtimeData.animatedBonesBuffer.Resize(boneBufferLen.x, NativeArrayOptions.UninitializedMemory);
		runtimeData.boneToEntityArr.Resize(boneBufferLen.x, NativeArrayOptions.UninitializedMemory);
		runtimeData.entityToDataOffsetMap.Capacity = math.max(boneBufferLen.x, runtimeData.entityToDataOffsetMap.Capacity);

		//	Clear flags by two resizes
		runtimeData.boneTransformFlagsHolderArr.Resize(0, NativeArrayOptions.UninitializedMemory);
		runtimeData.boneTransformFlagsHolderArr.Resize(boneBufferLen.y, NativeArrayOptions.ClearMemory);
		
		runtimeData.entityToDataOffsetMap.Clear();

		//	Fill boneToEntityArr with proper values
		var boneToEntityArrFillJob = new CalculatePerBoneInfoJob()
		{
			bonePosesOffsets = bonePosesOffsetsArr.AsArray(),
			boneToEntityIndices = runtimeData.boneToEntityArr.AsArray(),
			chunkBaseEntityIndices = chunkBaseEntityIndices,
			rigDefinitionTypeHandle = rigDefinitionTypeHandle,
			entities = entitiesArr.AsDeferredJobArray(),
			entityToDataOffsetMap = runtimeData.entityToDataOffsetMap.AsParallelWriter()
		};

		var boneToEntityJH = boneToEntityArrFillJob.ScheduleParallel(animatedObjectQuery, default);
		boneToEntityJH.Complete();
		return boneToEntityJH;
	}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	JobHandle AnimationCalculation(ref SystemState ss, NativeList<Entity> entitiesArr, in RuntimeAnimationData runtimeData, JobHandle dependsOn)
	{
		animationToProcessBufferLookup.Update(ref ss);
		rootMotionAnimationStateBufferLookupRW.Update(ref ss);

		var rigDefsArr = animatedObjectQuery.ToComponentDataListAsync<RigDefinitionComponent>(Allocator.TempJob, out var rigDefsLookupJH);
		var dataGatherJH = JobHandle.CombineDependencies(rigDefsLookupJH, dependsOn);

		var computeAnimationsJob = new ComputeBoneAnimationJob()
		{
			animationsToProcessLookup = animationToProcessBufferLookup,
			entityArr = entitiesArr.AsDeferredJobArray(),
			rigDefs = rigDefsArr.AsDeferredJobArray(),
			boneTransformFlagsArr = runtimeData.boneTransformFlagsHolderArr,
			animatedBonesBuffer = runtimeData.animatedBonesBuffer,
			boneToEntityArr = runtimeData.boneToEntityArr,
			rootMotionAnimStateBufferLookup = rootMotionAnimationStateBufferLookupRW,
		};

		var jh = computeAnimationsJob.ScheduleBatch(runtimeData.animatedBonesBuffer.Length, 16, dataGatherJH);
		rigDefsArr.Dispose(jh);

		return jh;
	}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	JobHandle ProcessUserCurves(ref SystemState ss, JobHandle dependsOn)
	{
		animationToProcessBufHandle.Update(ref ss);
		animatorParameterBufHandleRW.Update(ref ss);
		animatorParameterIndexTableHandle.Update(ref ss);

		var userCurveProcessJob = new ProcessUserCurvesJob();

		var jh = userCurveProcessJob.ScheduleParallel(dependsOn);
		return jh;
	}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	JobHandle CopyEntityBonesToAnimationTransforms(ref SystemState ss, ref RuntimeAnimationData runtimeData, JobHandle dependsOn)
	{
		rigDefinitionLookup.Update(ref ss);
		parentComponentLookup.Update(ref ss);
		localTransfromLookup.Update(ref ss);
			
		//	Now take available entity transforms as ref poses overrides
		var copyEntityBoneTransforms = new CopyEntityBoneTransformsToAnimationBuffer()
		{
			rigDefComponentLookup = rigDefinitionLookup,
			boneTransformFlags = runtimeData.boneTransformFlagsHolderArr,
			entityToDataOffsetMap = runtimeData.entityToDataOffsetMap,
			animatedBoneTransforms = runtimeData.animatedBonesBuffer,
			parentComponentLookup = parentComponentLookup,
			localTransformComponentLookup = localTransfromLookup
		};

		var jh = copyEntityBoneTransforms.ScheduleParallel(boneEntityQuery, dependsOn);
		return jh;
	}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[BurstCompile]
	public void OnUpdate(ref SystemState ss)
	{
		var entityCount = animatedObjectQuery.CalculateEntityCount();
		if (entityCount == 0) return;
		
		ref var runtimeData = ref SystemAPI.GetSingletonRW<RuntimeAnimationData>().ValueRW;

		bonePosesOffsetsArr.Resize(entityCount + 1, NativeArrayOptions.UninitializedMemory);
		var chunkBaseEntityIndices = animatedObjectQuery.CalculateBaseEntityIndexArrayAsync(Allocator.TempJob, ss.Dependency, out var baseIndexCalcJH);
		var entitiesArr = animatedObjectQuery.ToEntityListAsync(Allocator.TempJob, ss.Dependency, out var entityArrJH);

		var combinedJH = JobHandle.CombineDependencies(baseIndexCalcJH, entityArrJH);

		//	Define array with bone pose offsets for calculated bone poses
		var calcBoneOffsetsJH = PrepareComputationData(ref ss, chunkBaseEntityIndices, ref runtimeData, entitiesArr, combinedJH);

		//	User curve calculus
		var userCurveProcessJobHandle = ProcessUserCurves(ref ss, calcBoneOffsetsJH);

		//	Spawn jobs for animation calculation
		var computeAnimationJobHandle = AnimationCalculation(ref ss, entitiesArr, runtimeData, userCurveProcessJobHandle);

		//	Copy entities poses into animation buffer for non-animated parts
		var copyEntityTransformsIntoAnimationBufferJH = CopyEntityBonesToAnimationTransforms(ref ss, ref runtimeData, computeAnimationJobHandle);

		ss.Dependency = copyEntityTransformsIntoAnimationBufferJH;
		entitiesArr.Dispose(ss.Dependency);
	}
}
}
