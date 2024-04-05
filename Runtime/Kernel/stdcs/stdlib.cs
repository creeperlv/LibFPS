using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LibFPS.Kernel.stdcs.stdlib
{
    public unsafe static class stdlib
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]      
        public static void* malloc(int size)
        {
            return (void*)Marshal.AllocHGlobal(size);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void free(void* ptr)
        {
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* realloc(void* ptr, int size)
        {
            return (void*)Marshal.ReAllocHGlobal((IntPtr)ptr, (IntPtr)size);
        }
    }
    public unsafe static class @string
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* memcpy(void* dst, void* src, int size)
        {
            Buffer.MemoryCopy(src, dst, size, size);
            return dst;
        }
    }
}
