using Unity.Netcode;

namespace LibFPS.Level
{
	public class LevelLoader : NetworkBehaviour
	{
		[Rpc(SendTo.Server)]
		public void ReportLoadDoneRpc()
		{

		}
	}
}
