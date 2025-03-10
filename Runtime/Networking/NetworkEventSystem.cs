using LibFPS.Kernel;
using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace LibFPS.Networking
{
	public class NetworkEventSystem : NetworkBehaviour
	{
		Dictionary<int, Action<ulong, string>> EventListener = new Dictionary<int, Action<ulong, string>>();
		Dictionary<int, Action<ulong>> RequestListener = new Dictionary<int, Action<ulong>>();
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
		[Rpc(SendTo.Everyone)]
		public void SendEventToEveryBodyRpc(int Key, string data, RpcParams rpcParams)
		{
			ExecuteEventListeners(Key, data, rpcParams);
		}
		[Rpc(SendTo.Owner)]
		public void SendEventToOwnerRpc(int Key, string data, RpcParams rpcParams)
		{
			ExecuteEventListeners(Key, data, rpcParams);
		}
		[Rpc(SendTo.Server)]
		public void SendEventToServerRpc(int Key, string data, RpcParams rpcParams)
		{
			ExecuteEventListeners(Key, data, rpcParams);
		}
		[Rpc(SendTo.SpecifiedInParams)]
		public void SendEventToSomeoneRpc(int Key, string data, RpcParams rpcParams)
		{
			ExecuteEventListeners(Key, data, rpcParams);
		}
		[Rpc(SendTo.Everyone)]
		public void SendRequestToEveryBodyRpc(int Key,  RpcParams rpcParams)
		{
			ExecuteRequestListeners(Key, rpcParams);
		}
		[Rpc(SendTo.Owner)]
		public void SendRequestToOwnerRpc(int Key,  RpcParams rpcParams)
		{
			ExecuteRequestListeners(Key, rpcParams);
		}
		[Rpc(SendTo.Server)]
		public void SendRequestToServerRpc(int Key,  RpcParams rpcParams)
		{
			ExecuteRequestListeners(Key, rpcParams);
		}
		[Rpc(SendTo.SpecifiedInParams)]
		public void SendRequestToSomeoneRpc(int Key,  RpcParams rpcParams)
		{
			ExecuteRequestListeners(Key, rpcParams);
		}
		private void ExecuteEventListeners(int key, string data, RpcParams rpcParams)
		{
			if (this.EventListener.TryGetValue(key, out var action))
			{
				action(rpcParams.Receive.SenderClientId, data);
			}
			if (LevelCore.Instance.EventListener.TryGetValue(key, out action)){
				action(rpcParams.Receive.SenderClientId, data);
			}
		}
		private void ExecuteRequestListeners(int key, RpcParams rpcParams)
		{
			if (this.RequestListener.TryGetValue(key, out var action))
			{
				action(rpcParams.Receive.SenderClientId);
			}
			if (LevelCore.Instance.RequestListener.TryGetValue(key, out action)){
				action(rpcParams.Receive.SenderClientId);
			}
		}
	}
}
