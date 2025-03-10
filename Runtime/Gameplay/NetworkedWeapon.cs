using LibFPS.Gameplay.Data;
using LibFPS.Gameplay.Effects;
using LibFPS.Kernel;
using LibFPS.Kernel.DefinitionManagement;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class NetworkedWeapon : NetworkedPickupable
	{
		[Header("NetworkedPickupable")]
		public BipedPositionType PositionType;
		public Biped Holder;
		public BaseEntity HolderEntity;
		public NetworkVariable<float> Ammo;
		public NetworkVariable<bool> IsFireDown = new NetworkVariable<bool>(false, writePerm: NetworkVariableWritePermission.Owner);
		public Transform FirePoint = null;
		public Transform EffectPoint = null;
		public Transform CurrentFirePoint = null;
		public WeaponMode fireType;
		public bool IsHeavyWeapon;
		public bool WillUseTPSViewModel;
		public int BulletID;
		public int WeaponDef;
		public WeaponDef DefaultDef;
		public WeaponDef CurrentDef;
		public bool TryFire = false;
		public bool __tryFire = false;
		private float TimeSinceLastFire;
		private float CurrentScatter;
		private float CamShakeDecay;
		private float CamShakeSpeed;
		private float CameraShakeIntensity;
		public int FireEffect;
		public int CurrentMagazine;
		public int CurrentBackup;
		public string Animation_Fire;
		public string Animation_Reload;
		public List<Renderer> AmmoRenderers;
		public AmmoDisp AmmoDispType;
		int __semiCount = 0;
		public override void Start()
		{
			base.Start();
			StartCoroutine(QueryDef());
			NotifyWeaponAmmo();
		}
		public IEnumerator QueryDef()
		{
			while (true)
			{

				if (DefinitionManager.Instance != null)
				{
					if (DefinitionManager.Instance.WeaponDefDefinition.TryGetValue(WeaponDef, out var def))
					{
						CurrentDef = def;
					}
					else
					{
						CurrentDef = DefaultDef;
					}
					yield break;
				}
				yield return null;
			}
		}
		public override void OnUpdate()
		{
			switch (this.CurrentDef.WeaponMode)
			{
				case WeaponMode.Melee:
					break;
				case WeaponMode.SemiAuto:
					{
						if (TimeSinceLastFire > CurrentDef.MinimalFireInterval)
						{
							if (TryFire)
							{

								if (__tryFire == false || __semiCount < CurrentDef.SemiAutoBrustCount)
								{
									__tryFire = true;
									Fire();
								}
							}
							else if (__tryFire && __semiCount < CurrentDef.SemiAutoBrustCount)
							{
								Fire();
							}
							else if (__semiCount >= CurrentDef.SemiAutoBrustCount || CurrentMagazine <= 0)
							{
								__tryFire = false;
								__semiCount = 0;
							}
						}
					}
					break;
				case WeaponMode.Auto:
					{
						if (TimeSinceLastFire > CurrentDef.MinimalFireInterval)
						{
							if (TryFire)
								Fire();
						}
					}
					break;
				default:
					break;
			}
			TimeSinceLastFire += Time.deltaTime;
			if (CurrentScatter > 0)
			{
				CurrentScatter -= CurrentDef.ScatterRecover * Time.deltaTime;
			}
		}
		public void NotifyWeaponAmmo()
		{
			switch (AmmoDispType)
			{
				case AmmoDisp.None:
					break;
				case AmmoDisp.TwoDig:
					{
						AmmoRenderers[0].material.SetFloat("_DigitNum", CurrentMagazine % 10);
						AmmoRenderers[1].material.SetFloat("_DigitNum", Mathf.FloorToInt(CurrentMagazine / 10));
					}
					break;
				case AmmoDisp.ThreeDig:
					{

						AmmoRenderers[0].material.SetFloat("_DigitNum", CurrentMagazine % 10);
						AmmoRenderers[1].material.SetFloat("_DigitNum", Mathf.FloorToInt(CurrentMagazine / 10) % 10);
						AmmoRenderers[2].material.SetFloat("_DigitNum", Mathf.FloorToInt(CurrentMagazine / 100));
					}
					break;
				case AmmoDisp.Liner:
					break;
				case AmmoDisp.Text:
					break;
				default:
					break;
			}
		}
		void PlayFireEffect()
		{
			if (!LevelCore.Instance.IsNetworked())
			{

				__localFireEffect();
			}
			else
			{
				FireEffectRpc();
			}
		}
		void __localFireEffect()
		{

			LevelCore.Instance.SpawnEffectObjectLocal(FireEffect, EffectPoint);
		}
		[Rpc(SendTo.Everyone)]
		void FireEffectRpc()
		{
			__localFireEffect();
		}
		void Fire()
		{
			TimeSinceLastFire = 0;
			if (this.CurrentMagazine <= 0)
			{
				return;
			}
			LevelCore.Instance.SpawnBullet(BulletID, HolderEntity, CurrentFirePoint, CurrentScatter, this.CurrentDef.damageConfig);
			if (CurrentDef.WeaponMode == WeaponMode.SemiAuto)
			{
				if (__semiCount >= CurrentDef.SemiAutoBrustCount)
				{
					return;
				}
				__semiCount++;
			}
			PlayFireEffect();
			this.CurrentMagazine--;
			NotifyWeaponAmmo();
			if (Holder != null)
			{
				Holder.UpperAnimator.SetTrigger(Animation_Fire);
				CurrentScatter += CurrentDef.ScatterPerShot;
				if (CurrentScatter > CurrentDef.MaxScatter)
				{
					CurrentScatter = CurrentDef.MaxScatter;
				}
				var effect = Holder.GetComponentInChildren<CameraShakeEffect>();
				if (effect != null)
					effect.SetShake(1f, true, CamShakeDecay, true, CamShakeSpeed, CameraShakeIntensity, CameraShakeIntensity);
			}
		}
	}
	public enum AmmoDisp
	{
		None, TwoDig, ThreeDig, Liner,Text
	}
	public enum BipedPositionType
	{
		Main, Side, Heavy, HandOnly
	}
	public enum WeaponMode
	{
		Melee,
		SemiAuto,
		Auto,
	}
}