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
		public KVList<int, PhysicsHitDefinition> RawHitDefinition= new KVList<int, PhysicsHitDefinition>();
		public Dictionary<int, PhysicsHitDefinition> HitDefinition= new Dictionary<int, PhysicsHitDefinition>();
		public KVList<int, PhysicsSoundDefinition> RawPhysicsSoundDefinition = new KVList<int, PhysicsSoundDefinition>();
		public Dictionary<int, PhysicsSoundDefinition> PhysicsSoundDefinition = new Dictionary<int, PhysicsSoundDefinition>();
		public void Start()
		{
			Instance = this;
			WeaponDefDefinition=RawWeaponDefDefinition.ToDictionary();
			HitDefinition = RawHitDefinition.Map((a) => a, (b) => {
				b.Init();
				return (true, b);
			});
			PhysicsSoundDefinition = RawPhysicsSoundDefinition.Map((a) => a, (b) => {
				b.Init();
				return (true, b);
			});
		}
	}
}
