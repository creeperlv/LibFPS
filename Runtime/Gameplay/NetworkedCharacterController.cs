using LibFPS.Kernel;
using LibFPS.Kernel.ResourceManagement;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class NetworkedCharacterController : NetworkBehaviour
	{
		public Biped biped;
		public BaseEntity Entity;
		public bool isPlayerObject;
		public NetworkVariable<Vector2> MoveDirection = new NetworkVariable<Vector2>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
		public NetworkVariable<bool> WillRun = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
		public NetworkVariable<bool> WillCrouch = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
		public void Start()
		{
			if (LevelCore.Instance == null || !LevelCore.Instance.IsNetworked())
			{
				if (isPlayerObject)
					FPSController.Instance.NetCharacterController = this;
			}
			else
			{
				if (IsClient)
					if (isPlayerObject)
						FPSController.Instance.NetCharacterController = this;
			}
		}
		[Rpc(SendTo.Everyone)]
		public void ChangeAnimationRpc(string id)
		{
			biped.UpperAnimatorAnimationController = id;
		}
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
			biped.IsCrouch = WillCrouch.Value;
		}
		public void Jump()
		{
			if (IsClient == false && IsServer == false)
			{
				RealJump();
			}
			else if (IsHost)
			{
				RealJump();
			}
			else
			{
				JumpRpc();
			}
		}
		private void __use()
		{
			if (Entity.ActiveIntractableObjects.Count > 0)
			{
				Entity.ActiveIntractableObjects[0].Interact(this.gameObject.GetInstanceID());
				Entity.ActiveIntractableObjects.RemoveAt(0);
			}
		}
		public void Use()
		{

			if (IsClient == false && IsServer == false)
			{
				__use();
			}
			else if (IsHost)
			{
				__use();
			}
			else
			{
				UseRpc();
			}
		}
		[Rpc(SendTo.Server)]
		private void UseRpc()
		{
			__use();
		}
		private void RealJump()
		{
			biped.Jump();
		}
		[Rpc(SendTo.Server)]
		void JumpRpc()
		{
			RealJump();
		}
		public void Move(float h, float v)
		{
			this.MoveDirection.Value = new Vector2(h, v);
		}
	}
}