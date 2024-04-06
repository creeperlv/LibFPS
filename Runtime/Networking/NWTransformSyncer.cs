using UnityEngine;
namespace LibFPS.Networking
{
    public class NWTransformSyncer : MonoBehaviour, ISyncData
    {
        public unsafe int Length()
        {
            return sizeof(Vector3) + sizeof(Quaternion) + sizeof(Vector3);
        }

        public void Pack(SyncedRoot root)
        {
            root.PackStruct(transform.position);
            root.PackStruct(transform.rotation);
            root.PackStruct(transform.position);
        }
        Vector3 pos;
        Quaternion rot;
        Vector3 scal;
        public void Read(SyncedRoot root)
        {
            unsafe
            {
                Vector3 pos;
                Quaternion rot;
                Vector3 scal;
                root.ReadStruct(&pos);
                root.ReadStruct(&rot);
                root.ReadStruct(&scal);
                this.pos = pos;
                this.rot = rot;
                this.scal = scal;
            }
        }
        ulong thisIteration = 0;
        Vector3 _pos;
        Quaternion _rot;
        Vector3 _scal;
        public void Update()
        {
            if (thisIteration < Syncer.Instance.Iteration)
            {
                _pos = pos;
                _rot = rot;
                _scal = scal;
            }
            else
            {
                this.transform.position = Vector3.Lerp(_pos, pos, Syncer.Instance.LerpT);
                this.transform.rotation = Quaternion.Lerp(_rot, rot, Syncer.Instance.LerpT);
                this.transform.localScale = Vector3.Lerp(_scal, scal, Syncer.Instance.LerpT);
            }
        }
    }
}
