using Unity.Burst;
using Unity.Collections;
using Unity.Deformations;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

/////////////////////////////////////////////////////////////////////////////////

namespace Rukhanka
{

[DisableAutoCreation]
[RequireMatchingQueriesForUpdate]
partial struct AnimationApplicationSystem: ISystem
{
	EntityQuery
		boneObjectEntitiesWithParentQuery,
		boneObjectEntitiesNoParentQuery,
		skinnedMeshWithAnimatorQuery,
		rigDefinitionQuery;

	NativeParallelHashMap<Hash128, BlobAssetReference<BoneRemapTableBlob>> rigToSkinnedMeshRemapTables;

	BufferTypeHandle<SkinMatrix> skinMatrixBufHandleRW;

	ComponentTypeHandle<RigDefinitionComponent> rigDefinitionTypeHandle;
	ComponentTypeHandle<LocalTransform> localTransformTypeHandleRW;
	ComponentTypeHandle<LocalTransform> localTransformTypeHandle;
	ComponentTypeHandle<AnimatorEntityRefComponent> animatorEntityRefTypeHandle;
	ComponentTypeHandle<AnimatedSkinnedMeshComponent> animatedSkinnedMeshComponentTypeHandle;
	ComponentTypeHandle<Parent> parentComponentTypeHandle;

	ComponentLookup<RigDefinitionComponent> rigDefinitionComponentLookup;
	ComponentLookup<LocalTransform> localTransformComponentLookup;

	EntityTypeHandle entityTypeHandle;

/////////////////////////////////////////////////////////////////////////////////

	[BurstCompile]
	public void OnCreate(ref SystemState ss)
	{
		using var eqb0 = new EntityQueryBuilder(Allocator.Temp)
		.WithAll<AnimatorEntityRefComponent, Parent>()
		.WithAllRW<LocalTransform>();
		boneObjectEntitiesWithParentQuery = ss.GetEntityQuery(eqb0);

		using var eqb1 = new EntityQueryBuilder(Allocator.Temp)
		.WithAll<AnimatorEntityRefComponent>()
		.WithNone<Parent>()
		.WithAllRW<LocalTransform>();
		boneObjectEntitiesNoParentQuery = ss.GetEntityQuery(eqb1);

		using var eqb3 = new EntityQueryBuilder(Allocator.Temp)
		.WithAll<RigDefinitionComponent>();
		rigDefinitionQuery = ss.GetEntityQuery(eqb3);

		using var eqb4 = new EntityQueryBuilder(Allocator.Temp)
		.WithAll<SkinMatrix, AnimatedSkinnedMeshComponent>();
		skinnedMeshWithAnimatorQuery = ss.GetEntityQuery(eqb4);

		rigToSkinnedMeshRemapTables = new NativeParallelHashMap<Hash128, BlobAssetReference<BoneRemapTableBlob>>(128, Allocator.Persistent);

		rigDefinitionTypeHandle = ss.GetComponentTypeHandle<RigDefinitionComponent>(true);
		rigDefinitionComponentLookup = ss.GetComponentLookup<RigDefinitionComponent>(true);
		entityTypeHandle = ss.GetEntityTypeHandle();
		localTransformTypeHandleRW = ss.GetComponentTypeHandle<LocalTransform>();
		localTransformTypeHandle = ss.GetComponentTypeHandle<LocalTransform>(true);
		animatorEntityRefTypeHandle = ss.GetComponentTypeHandle<AnimatorEntityRefComponent>(true);
		skinMatrixBufHandleRW = ss.GetBufferTypeHandle<SkinMatrix>();
		parentComponentTypeHandle = ss.GetComponentTypeHandle<Parent>(true);
		animatedSkinnedMeshComponentTypeHandle = ss.GetComponentTypeHandle<AnimatedSkinnedMeshComponent>(true);
		localTransformComponentLookup = ss.GetComponentLookup<LocalTransform>(true);
	}
	
/////////////////////////////////////////////////////////////////////////////////

	[BurstCompile]
	public void OnDestroy(ref SystemState ss)
	{
		rigToSkinnedMeshRemapTables.Dispose();
	}

/////////////////////////////////////////////////////////////////////////////////

    [BurstCompile]
    public void OnUpdate(ref SystemState ss)
    {
		ref var runtimeData = ref SystemAPI.GetSingletonRW<RuntimeAnimationData>().ValueRW;

		FillRigToSkinBonesRemapTableCache(ref ss);

		//	Compute root motion
		var rootMotionJobHandle = ComputeRootMotion(ref ss, runtimeData, ss.Dependency);

		//	Propagate local animated transforms to the entities with parents
		var propagateTRSWithParentsJobHandle = PropagateAnimatedBonesToEntitiesTRS(ref ss, runtimeData, boneObjectEntitiesWithParentQuery, rootMotionJobHandle);

		//	Convert local bone transforms to absolute (root relative) transforms
		var makeAbsTransformsJobHandle = MakeAbsoluteBoneTransforms(ref ss, runtimeData, propagateTRSWithParentsJobHandle);

		//	Propagate absolute animated transforms to the entities without parents
		var propagateTRNoParentsJobHandle = PropagateAnimatedBonesToEntitiesTRS(ref ss, runtimeData, boneObjectEntitiesNoParentQuery, makeAbsTransformsJobHandle);

		//	Make corresponding skin matrices for all skinned meshes
		var applySkinJobHandle = ApplySkinning(ref ss, runtimeData, propagateTRNoParentsJobHandle);

		ss.Dependency = applySkinJobHandle;
    }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	JobHandle ComputeRootMotion(ref SystemState ss, in RuntimeAnimationData runtimeData, JobHandle dependsOn)
	{
		var computeRootMotionJob = new ComputeRootMotionJob()
		{
			animatedBonePoses = runtimeData.animatedBonesBuffer,
			entityToDataOffsetMap = runtimeData.entityToDataOffsetMap
		};

		var jh = computeRootMotionJob.ScheduleParallel(dependsOn);
		return jh;
	}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FillRigToSkinBonesRemapTableCache(ref SystemState ss)
	{
		rigDefinitionComponentLookup.Update(ref ss);

	#if RUKHANKA_DEBUG_INFO
		SystemAPI.TryGetSingleton<DebugConfigurationComponent>(out var dc);
	#endif

		var j = new FillRigToSkinBonesRemapTableCacheJob()
		{
			rigDefinitionArr = rigDefinitionComponentLookup,
			rigToSkinnedMeshRemapTables = rigToSkinnedMeshRemapTables,
			skinnedMeshes = skinnedMeshWithAnimatorQuery.ToComponentDataArray<AnimatedSkinnedMeshComponent>(Allocator.TempJob),
		#if RUKHANKA_DEBUG_INFO
			doLogging = dc.logAnimationCalculationProcesses
		#endif
		};

		j.Run();
		j.skinnedMeshes.Dispose();
	}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	JobHandle PropagateAnimatedBonesToEntitiesTRS(ref SystemState ss, in RuntimeAnimationData runtimeData, EntityQuery eq, JobHandle dependsOn)
	{
		rigDefinitionComponentLookup.Update(ref ss);

		var propagateAnimationJob = new PropagateBoneTransformToEntityTRSJob()
		{
			entityToDataOffsetMap = runtimeData.entityToDataOffsetMap,
			boneTransforms = runtimeData.animatedBonesBuffer,
			rigDefLookup = rigDefinitionComponentLookup,
		};

		var jh = propagateAnimationJob.ScheduleParallel(eq, dependsOn);
		return jh;
	}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	JobHandle MakeAbsoluteBoneTransforms(ref SystemState ss, in RuntimeAnimationData runtimeData, JobHandle dependsOn)
	{
		entityTypeHandle.Update(ref ss);
		rigDefinitionTypeHandle.Update(ref ss);

		var makeAbsTransformsJob = new MakeAbsoluteTransformsJob()
		{
			entityTypeHandle = entityTypeHandle,
			boneTransforms = runtimeData.animatedBonesBuffer,
			entityToDataOffsetMap = runtimeData.entityToDataOffsetMap,
			rigDefinitionTypeHandle = rigDefinitionTypeHandle,
			boneTransformFlags = runtimeData.boneTransformFlagsHolderArr
		};

		var jh = makeAbsTransformsJob.ScheduleParallel(rigDefinitionQuery, dependsOn);
		return jh;
	}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	JobHandle ApplySkinning(ref SystemState ss, in RuntimeAnimationData runtimeData, JobHandle dependsOn)
	{
		animatedSkinnedMeshComponentTypeHandle.Update(ref ss);
		rigDefinitionComponentLookup.Update(ref ss);
		skinMatrixBufHandleRW.Update(ref ss);

		var animationApplyJob = new ApplyAnimationToSkinnedMeshJob()
		{
			animatedSkinnedMeshHandle = animatedSkinnedMeshComponentTypeHandle,
			boneTransforms = runtimeData.animatedBonesBuffer,
			entityToDataOffsetMap = runtimeData.entityToDataOffsetMap,
			rigDefinitionLookup = rigDefinitionComponentLookup,
			rigToSkinnedMeshRemapTables = rigToSkinnedMeshRemapTables,
			skinMatrixBufHandle = skinMatrixBufHandleRW,
		};

		var jh = animationApplyJob.ScheduleParallel(skinnedMeshWithAnimatorQuery, dependsOn);
		return jh;
	}

}
}
