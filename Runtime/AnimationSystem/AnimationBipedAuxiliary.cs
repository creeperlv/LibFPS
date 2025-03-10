using LibFPS.Gameplay;
using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace LibFPS.AnimationSystem
{
	public class AnimationBipedAuxiliary : MonoBehaviour
	{
		public Biped biped;
		public BaseEntity Entity;
		public NetworkedCharacterController Controller;
		public DamageArea area;
		public void ApplyMelee()
		{
			area.gameObject.SetActive(true);
		}
		public void StopMelee()
		{
			area.gameObject.SetActive(false);
		}
		public void Reload()
		{
			if (Entity.WeaponInBag.Count > 0)
			{
				var Weapon = Entity.WeaponInBag[Entity.CurrentHoldingWeapon.Value];
				if (Weapon.CurrentMagazine < Weapon.CurrentDef.AmmoPerMagzine && Weapon.CurrentBackup > 0)
				{
					var desire = Weapon.CurrentDef.AmmoPerMagzine - Weapon.CurrentMagazine;
					int realLoaded = Math.Min(desire, Weapon.CurrentBackup);
					Weapon.CurrentMagazine += realLoaded;
					Weapon.CurrentBackup -= realLoaded;
					Weapon.NotifyWeaponAmmo();
				}
			}
		}
		public void Lock()
		{
			Controller.IsOperationLocked = true;
		}
		public void Unlock()
		{
			Controller.IsOperationLocked = false;

		}
	}
}
