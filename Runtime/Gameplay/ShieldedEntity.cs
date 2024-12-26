using Unity.Netcode;

namespace LibFPS.Gameplay
{
	public class ShieldedEntity : BaseEntity
	{
		public float MaxFloat;
		public NetworkVariable<float> Shield;

	}
}