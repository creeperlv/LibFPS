using Unity.Netcode;

namespace LibFPS.Gameplay
{
	public class NetworkedPickupable : AttachableObject, IInteractable
	{
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