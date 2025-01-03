using System;
using System.Collections.Generic;

namespace LibFPS.Kernel.Data
{
	[Serializable]
	public class KVPair<K, V>
	{
		public K Key;
		public V Value;
	}
	[Serializable]
	public class KVList<K, V>
	{
		public List<KVPair<K, V>> Data;
		public Dictionary<K, V> ToDictionary()
		{
			Dictionary<K, V> dict = new Dictionary<K, V>();
			foreach (var item in Data)
			{
				dict.Add(item.Key, item.Value);
			}
			return dict;
		}
		public Dictionary<A, B> Map<A, B>(Func<K, A> KeyMap, Func<V, B> ValueMap)
		{
			Dictionary<A, B> dict = new Dictionary<A, B>();
			foreach (var item in Data)
			{
				dict.Add(KeyMap(item.Key), ValueMap(item.Value));
			}
			return dict;
		}
	}
}
