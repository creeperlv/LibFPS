using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class NetworkedCharacterController : NetworkBehaviour
	{
		public Biped biped;
		public BaseEntity Entity;
		public NetworkVariable<Vector2> MoveDirection = new NetworkVariable<Vector2>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
		public NetworkVariable<bool> WillRun = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
		public List<Transform> BindableTransforms;
		public void Update()
		{
			if (IsClient == false && IsServer == false)
			{
				ApplyData();
			}
			else if (IsHost)
			{
				ApplyData();
			}
		}
		private void ApplyData()
		{

			biped.MoveDirection = MoveDirection.Value;
			biped.IsRunning = WillRun.Value;
		}
		public void Move(float h, float v)
		{
			this.MoveDirection.Value = new Vector2(h, v);
		}
	}
}