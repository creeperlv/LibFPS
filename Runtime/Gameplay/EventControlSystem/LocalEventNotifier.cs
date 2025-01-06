using UnityEngine;

namespace LibFPS.Gameplay.EventControlSystem
{
	public class LocalEventNotifier : MonoBehaviour, IEventNotifier
	{
		public void Hit(ulong HitterID)
		{
			EventController.Instance.OnHit.Invoke();
		}

		public void Kill(ulong HitterID)
		{
			EventController.Instance.OnKill.Invoke();
		}
	}
}
