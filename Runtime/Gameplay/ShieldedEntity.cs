using Unity.Netcode;

namespace LibFPS.Gameplay
{
	public class ShieldedEntity : BaseEntity
	{
		public NetworkVariable<float> Shield;
		public float MaxShield;

	}
}