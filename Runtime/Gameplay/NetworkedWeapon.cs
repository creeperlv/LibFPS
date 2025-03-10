using LibFPS.Gameplay.Data;
using LibFPS.Gameplay.Effects;
using LibFPS.Kernel;
using LibFPS.Kernel.DefinitionManagement;
using System.Collections;
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
		public float MaxAmmo;
		public NetworkVariable<float> Ammo;
		public NetworkVariable<bool> IsFireDown = new NetworkVariable<bool>(false, writePerm: NetworkVariableWritePermission.Owner);
		public Transform FirePoint = null;
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
		public string Animation_Fire;
		public string Animation_Reload;
		public override void Start()
		{
			base.Start();
			StartCoroutine(QueryDef());
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
								if (__tryFire == false)
								{
									__tryFire = true;
									Fire();
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
		void Fire()
		{
			TimeSinceLastFire = 0;
			LevelCore.Instance.SpawnBullet(BulletID, HolderEntity, CurrentFirePoint, CurrentScatter, this.CurrentDef.damageConfig);
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