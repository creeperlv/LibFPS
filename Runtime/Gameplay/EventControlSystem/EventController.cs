using UnityEngine;
using UnityEngine.Events;

namespace LibFPS.Gameplay.EventControlSystem
{
	/// <summary>
	/// Local-Only Event Controller
	/// </summary>
	public class EventController : MonoBehaviour
	{
		public static EventController Instance = null;
		public static IEventNotifier CurrentNotifier = null;
		public UnityEvent OnHit;
		public UnityEvent OnKill;
		void Start()
		{
			Instance = this;
		}
	}
}
