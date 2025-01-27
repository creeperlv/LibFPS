using System;
using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.Level
{
	public class LevelExecutor : MonoBehaviour
	{
		public Dictionary<string, ILevelEvent> events;
		public unsafe void Call(string name, IntPtr parameters, uint size, IntPtr ReturnValueAddr)
		{
			if (events.TryGetValue(name, out var __event))
			{
				__event.Execute(parameters, size, ReturnValueAddr);
			}
		}
	}
}
