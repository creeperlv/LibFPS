using Unity.Netcode;

namespace LibFPS.Gameplay
{
	public class NetworkedPickupable : NetworkBehaviour
	{
		[Rpc(SendTo.Server)]
		public void TryPickRpc(int BindedTransform, RpcParams rpcParams)
		{

		}
	}
}