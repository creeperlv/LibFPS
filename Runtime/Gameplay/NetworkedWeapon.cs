using LibFPS.Gameplay.Data;
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

					}
					break;
				case WeaponMode.Auto:
					{

					}
					break;
				default:
					break;
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