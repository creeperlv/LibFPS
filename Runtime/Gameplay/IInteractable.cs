namespace LibFPS.Gameplay
{
	public interface IInteractable
	{
		void Interact(Biped Actor);
		void InteractStart(Biped Actor);
		void InteractStop(Biped Actor);
	}
}