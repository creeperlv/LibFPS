using LibFPS.Gameplay;
using LibFPS.Gameplay.Data;
using UnityEngine;

namespace LibFPS.AnimationSystem
{
	public class DamageArea : MonoBehaviour
	{
		public BaseEntity Giver;
		public DamageConfig damage;
		public void OnTriggerStay(Collider other)
		{
			var be = other.GetComponentInParent<BaseEntity>();
			if (be != Giver)
			{
				be.DealDamage(damage);
			}
		}
		public void OnTriggerEnter(Collider other)
		{
			var be = other.GetComponentInParent<BaseEntity>();
			if (be != Giver)
			{
				be.DealDamage(damage);
			}
		}
	}
}
