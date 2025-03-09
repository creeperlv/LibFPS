using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
namespace LibFPS.Gameplay
{

	public class BaseEntity : NetworkBehaviour
	{
		public float MaxHP;
		public Biped Biped;
		public List<ActiveIntractableObject> ActiveIntractableObjects;
		public NetworkVariable<float> HP;
		public DeathBehaviour deathBehaviour;
		public int TargetReplacementObjectID;
		public List<MonoBehaviour> BehaviourToDisable;
		public int MaxNormalWeaponCanHold = 2;
		public NetworkVariable<int> CurrentHoldingWeapon = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		public List<NetworkedWeapon> WeaponInBag;
		private void Update()
		{
			if (WeaponInBag.Count > 0)
			{
				var currentWeapon = WeaponInBag[CurrentHoldingWeapon.Value];
				Biped.UpperAnimatorAnimationController = currentWeapon.AnimatorKey;
			}
		}
		public virtual void ChangeHP(float Amount)
		{
			HP.Value -= Amount;
			if (HP.Value <= 0)
			{
				Death();
			}
		}
		public void Death()
		{
			switch (deathBehaviour)
			{
				case DeathBehaviour.ReplaceObject:

					break;
				case DeathBehaviour.DeactiveBehaviours:
					break;
				default:
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddIntractable(ActiveIntractableObject intractable)
		{
			if (IsClient || !IsHost && !IsServer)
			{
				if (!ActiveIntractableObjects.Contains(intractable))
					ActiveIntractableObjects.Add(intractable);
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RemoveIntractable(ActiveIntractableObject intractable)
		{
			if (IsClient || !IsHost && !IsServer)
			{
				if (ActiveIntractableObjects.Contains(intractable))
					ActiveIntractableObjects.Remove(intractable);
			}
		}
		public bool TryGetCurrentWeapon(out NetworkedWeapon weapon)
		{
			weapon = null;
			if (WeaponInBag.Count <= 0) return false;
			if (CurrentHoldingWeapon.Value < 0) return false;
			weapon = WeaponInBag[CurrentHoldingWeapon.Value];
			return true;
		}
	}
	public enum DeathBehaviour
	{
		ReplaceObject,
		DeactiveBehaviours,
	}
}