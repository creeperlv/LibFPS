using LibFPS.Kernel.Data;
using UnityEngine;

namespace LibFPS.Level
{
	public class TriggerCollection : MonoBehaviour
	{
		public KVList<int, BaseTrigger> triggers;
		private void Awake()
		{

		}
	}
}
