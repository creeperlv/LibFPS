using LibFPS.Kernel.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.Level
{
	public class LevelExecutor : MonoBehaviour
	{
		public static LevelExecutor Instance;
		public void Awake()
		{
			Instance = this;
			events = SerializableEvents.Map((k) => k, v =>
			{
				if (v is GameObject go)
				{
					return (true, go.GetComponent<ILevelEvent>());

				}
				else if (v is ILevelEvent __event)
					return (true, __event);
				return (false, default);
			});
		}
		public KVList<ulong, UnityEngine.Object> SerializableEvents;
		public Dictionary<ulong, ILevelEvent> events;
		public unsafe void Call(ulong eventID, IntPtr parameters, uint size, IntPtr ReturnValueAddr)
		{
			if (events.TryGetValue(eventID, out var __event))
			{
				__event.Execute(parameters, size, ReturnValueAddr);
			}
		}
	}
}
