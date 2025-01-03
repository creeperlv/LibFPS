using Unity.Netcode;

namespace LibFPS.Gameplay
{
	public class NetworkedPickupable : AttachableObject
	{
		public string AnimatorKey;
		[Rpc(SendTo.Server)]
		public void TryPickRpc(int BindedTransform, RpcParams rpcParams)
		{

		}
	}
}