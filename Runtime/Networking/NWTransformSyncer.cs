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
                this.transform.position = pos;
                this.transform.rotation = rot;
                this.transform.localScale = scal;
            }
        }
    }
}
