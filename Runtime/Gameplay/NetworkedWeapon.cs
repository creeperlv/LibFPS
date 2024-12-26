using Unity.Netcode;

namespace LibFPS.Gameplay
{
	public class NetworkedWeapon : NetworkBehaviour
	{
		public float MaxAmmo;
		public NetworkVariable<float> Ammo;

	}
}