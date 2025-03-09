using LibFPS.Kernel.Data;
using Unity.Netcode;
using UnityEngine.Events;

namespace LibFPS.Gameplay
{
	public class ActiveIntractableObject : NetworkBehaviour
	{
		public LocalizedString Hint;
		private void OnTriggerEnter(UnityEngine.Collider other)
		{
			other.gameObject.GetComponent<BaseEntity>().AddIntractable(this);
		}
		private void OnTriggerExit(UnityEngine.Collider other)
		{
			other.gameObject.GetComponent<BaseEntity>().RemoveIntractable(this);
		}
	}
	public class PassiveIntractableObject : NetworkBehaviour
	{
	}
	public class IntractableObject : NetworkBehaviour
	{
		public UnityEvent<ulong> Action;
	}
}