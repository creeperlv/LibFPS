using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class NetworkedWeapon : NetworkedPickupable
	{
		[Header("NetworkedPickupable")]
		public WeaponType WeaponType;
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
	public enum WeaponType
	{
		Main, Side, Heavy
	}
	public enum WeaponMode
	{
		Melee,
		SemiAuto,
		Auto,
	}
}