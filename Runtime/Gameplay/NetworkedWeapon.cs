using Unity.Netcode;

namespace LibFPS.Gameplay
{
	public class NetworkedWeapon : NetworkedPickupable
	{
		public float MaxAmmo;
		public NetworkVariable<float> Ammo;
		public NetworkVariable<bool> IsFireDown = new NetworkVariable<bool>(false, writePerm: NetworkVariableWritePermission.Owner);
	}
	public enum WeaponMode
	{
		Melee,
		SemiAuto,
		Auto,
	}
}