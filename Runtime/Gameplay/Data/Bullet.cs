using LibFPS.Kernel;
using LibFPS.Kernel.Data;
using LibFPS.Kernel.DefinitionManagement;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.Gameplay.Data
{
	public class Bullet : MonoBehaviour
	{
		public bool IsHitScan;
		public bool IsMagnetic;
		public float MoveSpeed;
		public float TrackIntensity;
		public int HitEffect;
		public int BulletID;
		public BaseEntity Sender;
		public DamageConfig Damage;
		public void Update()
		{
			if (!IsMagnetic)
			{
				this.transform.Translate(this.transform.forward * (MoveSpeed * Time.deltaTime));
			}
		}
		public void OnCollisionEnter(Collision collision)
		{
			var be = collision.gameObject.GetComponentInParent<BaseEntity>();
			var mat = collision.gameObject.GetComponentInParent<PhysicMaterial>();
			if (be != null)
			{
				if (Sender != be)
				{
					be.DealDamage(Damage);
				}
			}
			if (DefinitionManager.Instance.HitDefinition.TryGetValue(this.BulletID, out var hitDef))
			{
				GameObject HitEffect = null;
				bool Hit = false;
				if (mat != null)
				{
					if (!hitDef.HitEffect.TryGetValue(mat.MaterialID, out HitEffect))
					{
						Hit = true;
					}
				}
				if (!Hit) Hit = hitDef.HitEffect.TryGetValue(this.HitEffect, out HitEffect);
				if (Hit)
				{
					var c = collision.GetContact(0);
					LevelCore.Instance.SpawnEffectObject(HitEffect, c.point, c.normal);
				}
			}
			Destroy(this);
		}
	}
	[Serializable]
	public class PhysicsHitDefinition
	{
		public KVList<int, GameObject> RawHitEffect;
		public Dictionary<int, GameObject> HitEffect;
		public void Init()
		{
			HitEffect = RawHitEffect.ToDictionary();
			RawHitEffect.Data.Clear();
		}
	}
	[Serializable]
	public class PhysicsSoundDefinition
	{
		public KVList<int, List<AudioClip>> RawSoundEffeect;
		public Dictionary<int, List<AudioClip>> SoundEffeect;
		public void Init()
		{
			SoundEffeect = RawSoundEffeect.ToDictionary();
			RawSoundEffeect.Data.Clear();
		}
	}
	[Serializable]
	public class PhysicMaterial:MonoBehaviour
	{
		public int MaterialID;
	}
	[Serializable]
	public class DamageConfig
	{
		public float HPDamage;
		public float ShieldDamage;
	}
	[Serializable]
	public class WeaponDef
	{
		public DamageConfig damageConfig;
		public bool IsUsingMagazine;
		public int MaxAmmoReserve;
		public int AmmoPerMagzine;
		public int SemiAutoBrustCount;
		public float MinimalFireInterval;
		public float MaxScatter;
		public float ScatterPerShot;
		public float ScatterRecover;
		public WeaponMode WeaponMode;
	}
}
