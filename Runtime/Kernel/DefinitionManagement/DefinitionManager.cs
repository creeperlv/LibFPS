using LibFPS.Gameplay.Data;
using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.Kernel.DefinitionManagement
{
	public class DefinitionManager : MonoBehaviour
	{
		public static DefinitionManager Instance;
		public Dictionary<int, WeaponDef> WeaponDefDefinition = new Dictionary<int, WeaponDef>();
		public void Start()
		{
			Instance = this;
		}
	}
}
