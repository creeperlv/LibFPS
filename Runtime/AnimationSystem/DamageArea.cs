using LibFPS.Gameplay;
using LibFPS.Gameplay.Data;
using UnityEngine;

namespace LibFPS.AnimationSystem
{
	public class DamageArea : MonoBehaviour
	{
		public BaseEntity Giver;
		public DamageConfig damage;
		public bool DealOverTime;
		public void OnTriggerStay(Collider other)
		{
			if (!DealOverTime) return;
			var be = other.GetComponentInParent<BaseEntity>();
			if (be != Giver)
			{
				be.DealDamageDT(damage);
			}
		}
		public void OnTriggerEnter(Collider other)
		{
			if (DealOverTime) return;
			var be = other.GetComponentInParent<BaseEntity>();
			if (be != Giver)
			{
				be.DealDamage(damage);
			}
		}
	}
}
