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
		public int RelationID;
		public float MaxHP;
		public float HPRegen;
		public float RegenDelay;
		public float HPDamageIntensity;
		public float ShieldDamageIntensity;
		public Biped Biped;
		public List<ActiveIntractableObject> ActiveIntractableObjects;
		public NetworkVariable<float> HP;
		public DeathBehaviour deathBehaviour;
		public int TargetReplacementObjectID;
		public float TimeToDestoryAfterDeath;
		public List<Rigidbody> RigidbodiesToEnableGravity;
		public List<Behaviour> BehaviourToDisable;
		public List<Behaviour> BehaviourToEnable;
		public int MaxNormalWeaponCanHold = 2;
		public Transform FirePoint;
		public bool UseEntityFirePoint;
		public NetworkVariable<int> CurrentHoldingWeapon = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		public List<NetworkedWeapon> WeaponInBag;
		public Action OnDeath;
		public Action OnBeingDamaged;
		protected float DamageTime = 0;
		bool __isdied = false;
		protected virtual void Update()
		{
			if (HP.Value < 0)
			{
				if (__isdied == false)
				{
					Death();
				}

			}
			if (WeaponInBag.Count > 0)
			{
				var currentWeapon = WeaponInBag[CurrentHoldingWeapon.Value];
				Biped.UpperAnimatorAnimationController = currentWeapon.AnimatorKey;
			}
			if (DamageTime < RegenDelay)
			{
				DamageTime += Time.deltaTime;
			}
			else
			{
				if (HP.Value < MaxHP)
					HP.Value += HPRegen * Time.deltaTime;
			}
		}
		public virtual void DealDamageDT(DamageConfig config)
		{
			DamageTime = 0;
			ChangeHP(config.HPDamage * HPDamageIntensity * Time.deltaTime + config.ShieldDamage * ShieldDamageIntensity * Time.deltaTime);
		}
		public virtual void DealDamage(DamageConfig config)
		{
			DamageTime = 0;
			ChangeHP(config.HPDamage * HPDamageIntensity + config.ShieldDamage * ShieldDamageIntensity);
			OnBeingDamaged?.Invoke();
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
			if (__isdied) return;
			__isdied = true;
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
						item.isKinematic = false;
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