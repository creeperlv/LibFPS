using LibFPS.Kernel.Data;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LibFPS.Localization
{
	public class LocaleProvider : MonoBehaviour
	{
		public KVList<string, TextAsset> LanguageFiles = new KVList<string, TextAsset>();
		public string fallbackLanguage;
		public Dictionary<string, string> CurrentLanguage = new Dictionary<string, string>();
		public static LocaleProvider Instance;
		void Start()
		{
			Instance = this;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string TryQueryString(LocalizedString localizedString)
		{
			return TryQueryString(localizedString.StringID, localizedString.Fallback);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string TryQueryString(string id, string fallback)
		{
			if (Instance == null) return fallback;
			return Instance.TryGetString(id, fallback);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string TryGetString(string id, string fallback)
		{
			if (CurrentLanguage.TryGetValue(id, out var text))
			{
				return text;
			}
			return fallback;
		}
	}
}
