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
		public NetworkVariable<bool> WillFire = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
		public bool IsOperationLocked = false;
		public void Start()
		{
			if (LevelCore.Instance == null || !LevelCore.Instance.IsNetworked())
			{
				if (isPlayerObject)
					if (FPSController.Instance != null)
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
			if (!IsOperationLocked)
				biped.IsRunning = WillRun.Value;
			biped.IsCrouch = WillCrouch.Value;
			if (!IsOperationLocked)
				if (Entity.TryGetCurrentWeapon(out var weapon))
				{
					weapon.TryFire = WillFire.Value;
				}
		}
		public void SwitchWeapon()
		{
			if (!IsOperationLocked)
				if (Entity.WeaponInBag.Count > 1)
				{
					Entity.CurrentHoldingWeapon.Value++;
					Entity.CurrentHoldingWeapon.Value = Entity.CurrentHoldingWeapon.Value % 2;
					var anotherIDX = (Entity.CurrentHoldingWeapon.Value + 1) % 2;
					var newW = Entity.WeaponInBag[Entity.CurrentHoldingWeapon.Value];
					var oldW = Entity.WeaponInBag[anotherIDX];
					biped.UpperAnimatorAnimationController = newW.AnimatorKey;
					biped.UpperAnimator.SetTrigger(biped.Pickup);
					if (biped.BindableDict.TryGetValue(BipedPositionType.HandOnly, out var t))
					{
						newW.TargetTransform = t;
					}
					if (biped.BindableDict.TryGetValue(oldW.PositionType, out t))
					{
						oldW.TargetTransform = t;
					}
				}
		}
		void __melee()
		{
			if (!IsOperationLocked)
				biped.UpperAnimator.SetTrigger(biped.Melee);

		}
		void __reload()
		{
			if (!IsOperationLocked)
				if (Entity.WeaponInBag.Count > 0)
				{
					var Weapon = Entity.WeaponInBag[Entity.CurrentHoldingWeapon.Value];
					if (Weapon.CurrentMagazine < Weapon.CurrentDef.AmmoPerMagzine && Weapon.CurrentBackup > 0)
					{
						biped.UpperAnimator.SetTrigger(biped.Reload);
					}

				}
		}
		public void Reload()
		{
			if (!LevelCore.Instance.IsNetworked())
			{
				__reload();
			}
		}
		public void Melee()
		{
			if (!LevelCore.Instance.IsNetworked())
			{
				__melee();
			}
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