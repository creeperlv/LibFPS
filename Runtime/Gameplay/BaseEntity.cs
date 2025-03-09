using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
namespace LibFPS.Gameplay
{

	public class BaseEntity : NetworkBehaviour
	{
		public float MaxHP;
		public List<ActiveIntractableObject> ActiveIntractableObjects;
		public NetworkVariable<float> HP;
		public DeathBehaviour deathBehaviour;
		public int TargetReplacementObjectID;
		public List<MonoBehaviour> BehaviourToDisable;
		public virtual void ChangeHP(float Amount)
		{
			HP.Value -= Amount;
			if (HP.Value <= 0)
			{
				Death();
			}
		}
		public void Death()
		{
			switch (deathBehaviour)
			{
				case DeathBehaviour.ReplaceObject:

					break;
				case DeathBehaviour.DeactiveBehaviours:
					break;
				default:
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddIntractable(ActiveIntractableObject intractable)
		{
			if (IsClient || !IsHost && !IsServer)
			{
				if (!ActiveIntractableObjects.Contains(intractable))
					ActiveIntractableObjects.Add(intractable);
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RemoveIntractable(ActiveIntractableObject intractable)
		{
			if (IsClient || !IsHost && !IsServer)
			{
				if (ActiveIntractableObjects.Contains(intractable))
					ActiveIntractableObjects.Remove(intractable);
			}
		}
	}
	public enum DeathBehaviour
	{
		ReplaceObject,
		DeactiveBehaviours,
	}
}