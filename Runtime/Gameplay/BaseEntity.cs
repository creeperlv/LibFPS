using LibFPS.Gameplay.Data;
using LibFPS.Kernel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
namespace LibFPS.Gameplay
{

	public class BaseEntity : NetworkBehaviour
	{
		public float MaxHP;
		public float HPDamageIntensity;
		public float ShieldDamageIntensity;
		public Biped Biped;
		public List<ActiveIntractableObject> ActiveIntractableObjects;
		public NetworkVariable<float> HP;
		public DeathBehaviour deathBehaviour;
		public int TargetReplacementObjectID;
		public float TimeToDestoryAfterDeath;
		public List<Rigidbody> RigidbodiesToEnableGravity;
		public List<MonoBehaviour> BehaviourToDisable;
		public List<MonoBehaviour> BehaviourToEnable;
		public int MaxNormalWeaponCanHold = 2;
		public NetworkVariable<int> CurrentHoldingWeapon = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		public List<NetworkedWeapon> WeaponInBag;
		public Action OnDeath;
		private void Update()
		{
			if (WeaponInBag.Count > 0)
			{
				var currentWeapon = WeaponInBag[CurrentHoldingWeapon.Value];
				Biped.UpperAnimatorAnimationController = currentWeapon.AnimatorKey;
			}
		}
		public virtual void DealDamage(DamageConfig config)
		{
			ChangeHP(config.HPDamage * HPDamageIntensity + config.ShieldDamage * ShieldDamageIntensity);
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
			OnDeath?.Invoke();
			switch (deathBehaviour)
			{
				case DeathBehaviour.ReplaceObject:
					var obj = LevelCore.Instance.SpawnObject(TargetReplacementObjectID, this.OwnerClientId);
					obj.transform.position = this.transform.position;
					obj.transform.rotation = this.transform.rotation;
					break;
				case DeathBehaviour.DeactiveBehaviours:
					foreach (var item in BehaviourToDisable)
					{
						item.enabled = false;
					}
					foreach (var item in RigidbodiesToEnableGravity)
					{
						item.useGravity = true;
					}
					break;
				default:
					break;
			}
			LevelCore.Instance.DestroyGameObejct(this.gameObject, TimeToDestoryAfterDeath);
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