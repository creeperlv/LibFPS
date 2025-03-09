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
		public KVList<string, int> spawnableObjectNames;
		public KVList<int, GameObject> spawnableObjects;
		private Dictionary<string, Dictionary<string, RuntimeAnimatorController>> AnimationControllers;
		private Dictionary<string, int> SpawnableObjectNames;
		private Dictionary<int, GameObject> SpawnableObjects;
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
			AnimationControllers = animations.Map((a) => a, (b) => (true, b.ToDictionary()));
			SpawnableObjects = spawnableObjects.ToDictionary();
			SpawnableObjectNames = spawnableObjectNames.ToDictionary();
		}
		public bool TryQuerySpawnableObjectsRecursively(string SpawnID, out GameObject gObj)
		{
			if (!SpawnableObjectNames.TryGetValue(SpawnID, out var id)){
				if (SpawnableObjects.TryGetValue(id, out gObj))
				{
					return true;
				}
			}
			foreach (var item in SubManagers)
			{
				if (item.TryQuerySpawnableObjectsRecursively(SpawnID, out gObj))
				{
					return true;
				}
			}
			gObj = null;
			return false;
		}
		public bool TryQuerySpawnableObjectsRecursively(int id, out GameObject gObj)
		{
			{
				if (SpawnableObjects.TryGetValue(id, out gObj))
				{
					return true;
				}
			}
			foreach (var item in SubManagers)
			{
				if (item.TryQuerySpawnableObjectsRecursively(id, out gObj))
				{
					return true;
				}
			}
			gObj = null;
			return false;
		}
		public bool TryQueryAnimationControllerRecursively(string CharacterID, string AnimationControllerKey, out RuntimeAnimatorController AnimatorController)
		{
			if (AnimationControllers.TryGetValue(CharacterID, out var animationControllers))
			{
				if (animationControllers.TryGetValue(AnimationControllerKey, out AnimatorController))
				{
					return true;
				}
			}
			foreach (var item in SubManagers)
			{
				if (item.TryQueryAnimationControllerRecursively(CharacterID, AnimationControllerKey, out AnimatorController))
				{
					return true;
				}
			}
			AnimatorController = null;
			return false;
		}
		public static bool TryQueryAnimationController(string CharacterID, string AnimationControllerKey, out RuntimeAnimatorController AnimatorController)
		{
			if (Instance == null)
			{
				AnimatorController = null;
				return false;
			}
			return Instance.TryQueryAnimationControllerRecursively(CharacterID, AnimationControllerKey, out AnimatorController);
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
