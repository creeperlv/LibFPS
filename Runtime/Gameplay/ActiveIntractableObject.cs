using LibFPS.Kernel.Data;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace LibFPS.Gameplay
{
	public class ActiveIntractableObject : IntractableObject
	{
		public LocalizedString Hint;
		private void OnTriggerEnter(UnityEngine.Collider other)
		{
			Debug.Log("Triggered.");
			var e = other.gameObject.GetComponentInParent<BaseEntity>();
			if (e != null)
				e.AddIntractable(this);
		}
		private void OnTriggerExit(UnityEngine.Collider other)
		{
			var e = other.gameObject.GetComponentInParent<BaseEntity>();
			if (e != null)
				e.RemoveIntractable(this);
		}
		public void OnCollisionEnter(UnityEngine.Collision collision)
		{
			var e = collision.gameObject.GetComponentInParent<BaseEntity>();
			if (e != null)
				e.AddIntractable(this);

		}
		public void OnCollisionExit(UnityEngine.Collision collision)
		{

			var e = collision.gameObject.GetComponentInParent<BaseEntity>();
			if (e != null)
				e.RemoveIntractable(this);
		}
	}
	public class PassiveIntractableObject : IntractableObject
	{
	}
	public class IntractableObject : NetworkBehaviour
	{
		public UnityEvent<int> Action;
		public void Interact(int Interactor)
		{
			Action.Invoke(Interactor);
		}
	}
}