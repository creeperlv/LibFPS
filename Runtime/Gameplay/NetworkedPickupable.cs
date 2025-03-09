using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class NetworkedPickupable : AttachableObject, IInteractable
	{
		public ActiveIntractableObject Interactable;
		[Header("NetworkedPickupable")]
		public string AnimatorKey;

		public void Interact(Biped Actor)
		{
		}

		public void InteractStart(Biped Actor)
		{
		}

		public void InteractStop(Biped Actor)
		{
		}
	}
}