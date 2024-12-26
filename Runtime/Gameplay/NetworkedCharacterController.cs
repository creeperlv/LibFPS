using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class NetworkedCharacterController : NetworkBehaviour
	{
		public Transform Head;
		public Transform Self;
		public List<Transform> BindableTransforms;
		[Rpc(SendTo.Server)]
		public void MoveRpc(Vector2 Input)
		{

		}
		[Rpc(SendTo.Server)]
		public void LookRpc(float Horizontal,float Vertical)
		{

		}
	}
}