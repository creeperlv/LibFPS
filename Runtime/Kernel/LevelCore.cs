using LibFPS.Gameplay;
using LibFPS.Gameplay.Data;
using LibFPS.Kernel.DefinitionManagement;
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
		public LayerMask ExcludeAirBlock;
		public LayerMask ExcludePlauerAndAirBlock;
		public Dictionary<int, BaseEntity> ManagedEntities = new Dictionary<int, BaseEntity>();
		public void Start()
		{
			Instance = this;
			PlayerObjectLayer = LayerMask.NameToLayer(PlayerObjectLayerName);
			NormalLayer = LayerMask.NameToLayer(NormalLayerName);
		}
		public void SpawnBullet(int ID, BaseEntity Sender, Transform pos, float RandomizeIntensity, DamageConfig damageConfig, float HitScanRange = 200f)
		{
			if (ResourceManagement.ResourceManager.Instance.TryQuerySpawnableObjectsRecursively(ID, out var gObj))
			{
				Quaternion Rotation = this.transform.rotation;
				if (IsNetworked())
				{
					//TODO
				}
				else
				{
					if (gObj.TryGetComponent<Bullet>(out var b))
					{
						if (b.IsHitScan)
						{
							Debug.DrawRay(pos.position, pos.forward, Color.red, 10);
							if (Physics.Raycast(pos.position, pos.forward, out var hit, HitScanRange, this.ExcludeAirBlock, QueryTriggerInteraction.Ignore))
							{
								Debug.Log("Hit Scan hit");
								var bEntity = hit.collider.GetComponentInParent<BaseEntity>();
								if (bEntity != null)
								{

									if (Sender != bEntity)
									{
										bEntity.DealDamage(damageConfig);
									}
								}
								if (DefinitionManager.Instance.HitDefinition.TryGetValue(b.BulletID, out var hitDef))
								{
									GameObject HitEffect = null;
									bool Hit = false;
									var mat = hit.collider.GetComponentInParent<Gameplay.Data.PhysicMaterial>();
									if (mat != null)
									{
										if (!hitDef.HitEffect.TryGetValue(mat.MaterialID, out HitEffect))
										{
											Hit = true;
										}
									}
									if (!Hit) Hit = hitDef.HitEffect.TryGetValue(b.HitEffect, out HitEffect);
									if (Hit)
									{
										LevelCore.Instance.SpawnEffectObject(HitEffect, hit.point, hit.normal);
									}
									Debug.Log("Found Hit Effect=" + Hit);
								}
							}
							return;
						}
					}
					var __object = Instantiate(gObj);
					__object.transform.position = pos.position;
					__object.transform.rotation = pos.rotation;
					var rb = __object.GetComponent<Bullet>();
					if (rb != null)
					{
						rb.Damage = damageConfig;
					}
				}
			}
		}
		public void SpawnEffectObjectLocal(int ID, Transform pos)
		{
			if (ResourceManagement.ResourceManager.Instance.TryQuerySpawnableObjectsRecursively(ID, out var gObj))
			{
				var __object = Instantiate(gObj, pos);
			}
		}
		public void SpawnEffectObject(int ID, Transform pos)
		{
			if (ResourceManagement.ResourceManager.Instance.TryQuerySpawnableObjectsRecursively(ID, out var gObj))
			{
				SpawnEffectObject(gObj, pos.position, pos.rotation);
			}

		}
		public void SpawnEffectObject(GameObject gObj, Vector3 Pos, Quaternion Rot)
		{
			if (IsNetworked())
			{
				var __object = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(gObj.GetComponent<NetworkObject>());
				__object.transform.position = Pos;
				__object.transform.rotation = Rot;
			}
			else
			{
				var __object = Instantiate(gObj);
				__object.transform.position = Pos;
				__object.transform.rotation = Rot;
			}
		}
		public void SpawnEffectObject(GameObject gObj, Vector3 Pos, Vector3 Face)
		{
			if (IsNetworked())
			{
				var __object = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(gObj.GetComponent<NetworkObject>());
				__object.transform.position = Pos;
				__object.transform.forward = Face;
			}
			else
			{
				var __object = Instantiate(gObj);
				__object.transform.position = Pos;
				__object.transform.forward = Face;
			}
		}
		public GameObject SpawnObject(int ID,Transform point, ulong OwnerID = 0)
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
					var __object = Instantiate(gObj, point.position, point.rotation);
					RecordObject(__object);
					return __object;
				}
			}
			return null;
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
					for (int i = 0; i < be.MaxNormalWeaponCanHold; i++)
					{
						if (be.WeaponInBag[i].WeaponDef == nWeapon.WeaponDef)
						{
							be.CurrentHoldingWeapon.Value = i;
							break;
						}
					}
					ReleasePickupable(be.WeaponInBag[be.CurrentHoldingWeapon.Value]);
					be.WeaponInBag.RemoveAt(be.CurrentHoldingWeapon.Value);
				}
				else
				{
					if (be.WeaponInBag.Count > 0)
					{
						if (be.WeaponInBag[0].WeaponDef == nWeapon.WeaponDef)
						{
							ReleasePickupable(be.WeaponInBag[0]);
							be.WeaponInBag.RemoveAt(0);
						}
					}
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
				if (be.UseEntityFirePoint)
				{
					nWeapon.CurrentFirePoint = be.FirePoint;
				}
				biped.IsPickingUp = true;
				pickupable.TargetTransform = value;
				be.WeaponInBag.Add(nWeapon);
				nWeapon.Holder = biped;
				nWeapon.HolderEntity = be;
				pickupable.TogglePickupable(false);
			}
		}
		public void DestroyGameObejct(GameObject obj, float Time)
		{
			if (ManagedEntities.ContainsKey(obj.GetInstanceID()))
			{
				ManagedEntities.Remove(obj.GetInstanceID());
			}
			{
				Destroy(obj, Time);
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
				if (nw.CurrentMagazine <= 0 && nw.CurrentBackup <= 0)
				{
					Destroy(p.gameObject);
				}
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
