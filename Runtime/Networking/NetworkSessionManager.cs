using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace LibFPS.Networking
{
	public class NetworkSessionManager : MonoBehaviour
	{
		public static NetworkSessionManager instance;
		public NetworkManager NetworkManager;
		public UnityTransport transport;
		public IConnectionDataProvider connectionDataProvider;
		public void Awake()
		{
			instance = this;
		}
		public void CreateLobby(ushort Port, bool RequireApproval)
		{
			NetworkManager.NetworkConfig.ConnectionApproval = RequireApproval;
			transport.ConnectionData.Port = Port;
			NetworkManager.StartHost();
		}
		public void ConnectToLobby(string Address, ushort Port)
		{
			transport.ConnectionData.Address = Address;
			transport.ConnectionData.Port = Port;
			NetworkManager.StartClient();
		}
	}
	public interface IConnectionDataProvider
	{
		string GetDisplayName();
	}
}
