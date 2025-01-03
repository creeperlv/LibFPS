using LibFPS.Kernel.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.Kernel.ResourceManagement
{
	public class ResourceManager : MonoBehaviour
	{
		public static ResourceManager Instance = null;
		public bool PersistentResource;
		public ResourceManagerLifetime resourceManagerLifetime;
		public KVList<string, KVList<string, RuntimeAnimatorController>> animations;
		public List<ResourceManager> SubManagers;
		[NonSerialized]
		[HideInInspector]
		public ResourceManager Parent;
		void Start()
		{
			if (PersistentResource)
			{
				DontDestroyOnLoad(this.gameObject);
			}
			switch (resourceManagerLifetime)
			{
				case ResourceManagerLifetime.ExclusiveManager:
					if (Instance != null)
					{
						Destroy(Instance);
					}
					Instance = this;
					break;
				case ResourceManagerLifetime.MergeIntoCurrent:
					if (Instance != null)
					{
						Instance.SubManagers.Add(this);
					}
					else
					{
						Instance = this;
					}
					break;
				default:
					break;
			}

		}
		private void OnDestroy()
		{
			if (resourceManagerLifetime == ResourceManagerLifetime.MergeIntoCurrent)
			{
				if (Instance != this)
				{
					try
					{
						Instance.SubManagers.Remove(this);
					}
					catch (Exception)
					{
					}
				}
			}
		}

	}
	public enum ResourceManagerLifetime
	{
		ExclusiveManager,
		MergeIntoCurrent,
	}
}
