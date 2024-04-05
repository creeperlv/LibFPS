using LibFPS.Kernel.stdcs.stdlib;
using LibFPS.Kernel.UnmanagedData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
namespace LibFPS.Networking
{
    public unsafe class SyncedRoot : MonoBehaviour
    {
        byte* data = null;
        public List<ISyncData> syncData;
        int L;
        public void AnalysisDataPack()
        {
            int L = Length();
            this.L = L;
            if (data != null)
            {
                data = (byte*)stdlib.realloc(data, L);
            }
            else
                data = (byte*)stdlib.malloc(L);
        }
        int Offset = 0;
        public void Pack()
        {
            Offset = 0;
            foreach (var item in syncData)
            {
                item.Pack(this);
            }
        }
        public unsafe void PackStruct<T>(T t) where T : unmanaged
        {
            @string.memcpy(data + Offset, &t, sizeof(T));
            Offset += sizeof(T);
        }
        public unsafe void ReadStruct<T>(T* t) where T : unmanaged
        {
            @string.memcpy(&t, data + Offset, sizeof(T));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Length()
        {
            int L = 0;
            foreach (var item in syncData)
            {
                L += item.Length();
            }
            return L;
        }

        public void Read()
        {
        }
    }
    public interface ISyncData
    {
        int Length();
        void Pack(SyncedRoot root);
        void Read(SyncedRoot root);
    }
}
