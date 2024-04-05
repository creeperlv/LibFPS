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
        float timeD;
        public void Update()
        {
            if (thisIteration < Syncer.Instance.Iteration)
            {
                timeD = 0;
                _pos = pos;
                _rot = rot;
                _scal = scal;
            }
            else
            {
                float t = Syncer.Instance.SyncInteval / timeD;
                this.transform.position = Vector3.Lerp(_pos, pos, t);
                this.transform.rotation = Quaternion.Lerp(_rot, rot, t);
                this.transform.localScale = Vector3.Lerp(_scal, scal, t);
            }
        }
    }
}
