using LibFPS.Kernel.Data;
using LibFPS.Scripting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.Level
{
	public class LevelExecutor : MonoBehaviour
	{
		public static LevelExecutor Instance;
		public VMBridge VMBridge;
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
		public KVList<ushort, UnityEngine.Object> SerializableEvents;
		public Dictionary<ushort, ILevelEvent> events;
		public unsafe void Call(ushort eventID, IntPtr parameters, uint size, IntPtr ReturnValueAddr)
		{
			if (events.TryGetValue(eventID, out var __event))
			{
				__event.Execute(parameters, size, ReturnValueAddr);
			}
		}
	}
}
