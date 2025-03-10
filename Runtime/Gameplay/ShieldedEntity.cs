using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class ShieldedEntity : BaseEntity
	{
		public NetworkVariable<float> Shield;
		public float MaxShield;
		public float ShieldRegen;
		protected override void Update()
		{
			base.Update();
			if (DamageTime > RegenDelay)
			{
				if (Shield.Value < MaxShield)
				{
					Shield.Value += ShieldRegen * Time.deltaTime;
				}
			}
		}
	}
}