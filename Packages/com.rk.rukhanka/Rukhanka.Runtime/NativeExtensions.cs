using System;
using Unity.Burst.Intrinsics;
using Unity.Collections.LowLevel.Unsafe;

/////////////////////////////////////////////////////////////////////////////////

namespace Rukhanka
{
public static class NativeExtensions
{
	public static unsafe void SetBitThreadSafe(this UnsafeBitArray ba, int pos)
    {
#if UNITY_BURST_EXPERIMENTAL_ATOMIC_INTRINSICS
		var idx = pos >> 6;
		var shift = pos & 0x3f;
		var value = 1ul << shift;
		Common.InterlockedOr(ref UnsafeUtility.AsRef<ulong>(ba.Ptr + idx), value);
#endif
    }

/////////////////////////////////////////////////////////////////////////////////

    public static unsafe Span<T> AsSpan<T>(this UnsafeList<T> l) where T: unmanaged
    {
	    var rv = new Span<T>(l.Ptr, l.Length);
	    return rv;
    }
}
}
