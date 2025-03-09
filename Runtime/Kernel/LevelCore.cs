using LibFPS.Gameplay;
using LibFPS.Kernel.Data;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace LibFPS.Kernel
{
	public class LevelCore : MonoBehaviour
	{
		public static LevelCore Instance;
		public int CurrentSpawnPoint;
		public List<Transform> SpawnPoints;
		internal Dictionary<int, Action<ulong, string>> EventListener = new Dictionary<int, Action<ulong, string>>();
		internal Dictionary<int, Action<ulong>> RequestListener = new Dictionary<int, Action<ulong>>();
		List<BaseEntity> ManagedEntities = new List<BaseEntity>();
		public void Start()
		{
			Instance = this;
		}
		public GameObject SpawnObject(int ID)
		{
			if (ResourceManagement.ResourceManager.Instance.TryQuerySpawnableObjectsRecursively(ID, out var gObj))
			{
				if (IsNetworked())
				{
					var __object = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(gObj.GetComponent<NetworkObject>());

					return __object.gameObject;
				}
				else
				{
					var __object = Instantiate(gObj);
					return __object;
				}
			}
			return null;
		}
		public void RecordObject(GameObject obj)
		{
			if (obj.TryGetComponent<BaseEntity>(out var be))
			{
				ManagedEntities.Add(be);
			}
			if (obj.TryGetComponent<NetworkedCharacterController>(out var cc))
			{
				if (cc.isPlayerObject)
				{
					cc.transform.position = SpawnPoints[CurrentSpawnPoint].position;
				}
			}
		}
		/// <summary>
		/// Check if is current in a network session.
		/// </summary>
		/// <returns></returns>
		public bool IsNetworked()
		{
			if (NetworkManager.Singleton == null)
			{
				return false;
			}
			if (NetworkManager.Singleton.IsConnectedClient || NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
			{
				return true;
			}
			return false;
		}
		public void RegisterEventListener(int key, Action<ulong, string> listener)
		{
			if (EventListener.ContainsKey(key))
			{
				EventListener[key] = listener;
			}
			else
			{
				EventListener.Add(key, listener);
			}
		}
		public void RegisterRequestListener(int key, Action<ulong> listener)
		{
			if (EventListener.ContainsKey(key))
			{
				RequestListener[key] = listener;
			}
			else
			{
				RequestListener.Add(key, listener);
			}
		}
		public void UnregisterEventListener(int key) { EventListener.Remove(key); }
		public void UnregisterRequestListener(int key) { RequestListener[key] = null; }
	}
}
