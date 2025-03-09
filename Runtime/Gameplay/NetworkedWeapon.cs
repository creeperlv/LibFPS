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
		public Transform FirePoint;
		public bool IsHeavyWeapon;
		public bool WillUseTPSViewModel;
		public int BulletID;
		public int WeaponDef;
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