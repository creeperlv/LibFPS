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
		public string PlayerObjectLayerName;
		public int PlayerObjectLayer;
		public int NormalLayer;
		public string NormalLayerName;
		Dictionary<int, BaseEntity> ManagedEntities = new Dictionary<int, BaseEntity>();
		public void Start()
		{
			Instance = this;
			PlayerObjectLayer = LayerMask.NameToLayer(PlayerObjectLayerName);
			NormalLayer = LayerMask.NameToLayer(NormalLayerName);
		}
		public GameObject SpawnObject(int ID, ulong OwnerID = 0)
		{
			if (ResourceManagement.ResourceManager.Instance.TryQuerySpawnableObjectsRecursively(ID, out var gObj))
			{
				if (IsNetworked())
				{
					var __object = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(gObj.GetComponent<NetworkObject>(), ownerClientId: OwnerID);
					RecordObject(__object.gameObject);
					return __object.gameObject;
				}
				else
				{
					var __object = Instantiate(gObj);
					RecordObject(__object);
					return __object;
				}
			}
			return null;
		}
		public void RecordObject(GameObject obj)
		{
			if (obj.TryGetComponent<BaseEntity>(out var be))
			{
				ManagedEntities.Add(obj.GetInstanceID(), be);
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
		public void Pickup(int EntityID, NetworkedPickupable pickupable)
		{
			foreach (var item in ManagedEntities.Keys)
			{
				Debug.Log($"ID:{item}=={EntityID}");
			}
			if (!ManagedEntities.TryGetValue(EntityID, out var be))
			{
				Debug.Log("Target Entity does not exist!");
				return;
			}
			if (!be.TryGetComponent<Biped>(out var biped))
			{
				Debug.Log("Biped does not exist on target entity!");
				return;
			}
			if (!be.TryGetComponent<NetworkedCharacterController>(out var ncc))
			{
				Debug.Log("NetworkCharacterController does not exist on target entity!");
				return;
			}

			if (pickupable is NetworkedWeapon nWeapon)
			{

				int wCount = 0;
				foreach (var item in be.WeaponInBag)
				{
					switch (item.PositionType)
					{
						case BipedPositionType.Main:
						case BipedPositionType.Side:
							wCount++;
							break;
						case BipedPositionType.Heavy:
							break;
						case BipedPositionType.HandOnly:
							break;
						default:
							break;
					}
				}
				if (wCount >= be.MaxNormalWeaponCanHold)
				{
					for (int i = be.WeaponInBag.Count - 1; i >= be.MaxNormalWeaponCanHold; i--)
					{
						ReleasePickupable(be.WeaponInBag[i]);
						be.WeaponInBag.RemoveAt(i);
					}
					ReleasePickupable(be.WeaponInBag[be.CurrentHoldingWeapon.Value]);
					be.WeaponInBag.RemoveAt(be.CurrentHoldingWeapon.Value);
				}
				if (be.CurrentHoldingWeapon.Value < 0)
				{
					be.CurrentHoldingWeapon.Value = 0;
				}
				if (!biped.BindableDict.TryGetValue(BipedPositionType.HandOnly, out var value))
				{
					Debug.Log("Biped does not have a hand!");
					return;
				}
				if (ncc == FPSController.Instance.NetCharacterController)
				{
					SetLayerRecursively(pickupable.gameObject, PlayerObjectLayer);
				}
				pickupable.TargetTransform = value;
				be.WeaponInBag.Add(nWeapon);
				nWeapon.Holder = biped;
				pickupable.TogglePickupable(false);
			}
		}
		public void SetLayerRecursively(GameObject obj, LayerMask mask)
		{
			obj.layer = mask;
			for (int i = 0; i < obj.transform.childCount; i++)
			{
				SetLayerRecursively(obj.transform.GetChild(i).gameObject, mask);
			}
		}
		public void ReleasePickupable(NetworkedPickupable p)
		{
			p.TargetTransform = null;
			if (p is NetworkedWeapon nw)
			{
				nw.Holder = null;
			}
			SetLayerRecursively(p.gameObject, NormalLayer);
			p.TogglePickupable(true);
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
