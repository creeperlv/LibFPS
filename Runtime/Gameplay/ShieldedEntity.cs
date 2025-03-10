using LibFPS.Gameplay.Data;
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
		public override void DealDamageDT(DamageConfig config)
		{
			DamageTime = 0;
			var shield = config.ShieldDamage * ShieldDamageIntensity * Time.deltaTime;
			if (Shield.Value > 0)
			{
				Shield.Value -= shield;
			}
			if (Shield.Value <= 0)
				ChangeHP(config.HPDamage * HPDamageIntensity * Time.deltaTime);
		}
		public override void DealDamage(DamageConfig config)
		{
			DamageTime = 0;
			var shield = config.ShieldDamage * ShieldDamageIntensity;
			if (Shield.Value > 0)
			{
				Shield.Value -= shield;
			}
			if (Shield.Value <= 0)
				ChangeHP(config.HPDamage * HPDamageIntensity);
		}
	}
}