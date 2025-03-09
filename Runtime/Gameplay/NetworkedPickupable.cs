using LibFPS.Kernel;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class NetworkedPickupable : AttachableObject
	{
		public ActiveIntractableObject Interactable;
		[Header("NetworkedPickupable")]
		public string AnimatorKey;
		public Action<int> InteractEvent;
		public Rigidbody Rigidbody;
		public List<Collider> Colliders;
		/// <summary>
		/// true means able to pickup.
		/// </summary>
		/// <param name="v"></param>
		[Rpc(SendTo.Everyone)]
		public void TogglePickupableRpc(bool v)
		{
			Rigidbody.useGravity = v;
			Rigidbody.isKinematic = !v;
			foreach (var item in Colliders)
			{
				item.enabled = v;
			}
		}
		public virtual void Start()
		{
			this.Interactable.Action.AddListener((id) => InteractEvent?.Invoke(id));
			this.Interactable.Action.AddListener((id) =>
			{
				if (LevelCore.Instance != null)
				{
					LevelCore.Instance.Pickup(id, this);
				}
			});
		}
	}
}