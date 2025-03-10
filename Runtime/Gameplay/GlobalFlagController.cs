using LibFPS.Kernel.Data;
using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.Gameplay
{
	public class GlobalFlagController : MonoBehaviour
	{
		public static GlobalFlagController Instance;
		public Dictionary<string, bool> Flags = new Dictionary<string, bool>();
		public KVList<string, bool> PredefinedFlags = new KVList<string, bool>();
		public void Start()
		{
			Instance = this;
			Flags = PredefinedFlags.ToDictionary();
		}
		public static void SetFlag(string key, bool fallback)
		{
			if (Instance != null)
			{
				if (Instance.Flags.ContainsKey(key))
					Instance.Flags[key] = fallback;
				else Instance.Flags.Add(key, fallback);
			}
		}
		public static bool QueryFlag(string key, bool fallback)
		{
			if (Instance == null) return fallback;
			if (Instance.Flags.TryGetValue(key, out var v))
			{
				return v;
			}
			return fallback;
		}
	}
}