using System.Numerics;
using Unity.Netcode;

namespace LibFPS.Gameplay
{
	public class NetworkedCharacterController : NetworkBehaviour
	{
		[Rpc(SendTo.Server)]
		public void MoveRpc(Vector2 Input)
		{

		}
	}
}