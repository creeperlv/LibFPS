using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace LibFPS.Gameplay
{

	public class BaseEntity : NetworkBehaviour
	{
		public float MaxHP;
		public NetworkVariable<float> HP;
		public DeathBehaviour deathBehaviour;
		public int TargetReplacementObjectID;
		public List<MonoBehaviour> BehaviourToDisable;
	}
	public enum DeathBehaviour
	{
		ReplaceObject,
		DeactiveBehaviours,
	}
}