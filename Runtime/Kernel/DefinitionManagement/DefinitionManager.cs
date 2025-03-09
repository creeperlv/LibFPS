using LibFPS.Gameplay.Data;
using LibFPS.Kernel.Data;
using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.Kernel.DefinitionManagement
{
	public class DefinitionManager : MonoBehaviour
	{
		public static DefinitionManager Instance;
		public KVList<int, WeaponDef> RawWeaponDefDefinition = new KVList<int, WeaponDef>();
		public Dictionary<int, WeaponDef> WeaponDefDefinition = new Dictionary<int, WeaponDef>();
		public void Start()
		{
			Instance = this;
			WeaponDefDefinition=RawWeaponDefDefinition.ToDictionary();
		}
	}
}
