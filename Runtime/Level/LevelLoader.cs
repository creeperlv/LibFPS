using Unity.Netcode;
using UnityEngine;

namespace LibFPS
{
    public class LevelLoader : NetworkBehaviour
    {
        [Rpc(SendTo.Server)]
        public void ReportLoadDoneRpc()
        {

        }
    }
}
