using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace LibFPS
{
	public class TestUI : MonoBehaviour
	{
		public InputField Address;
		public InputField Port;
		public Button Host;
		public Button Connect;
		public UnityTransport transport;
		void Start()
		{
			Connect.onClick.AddListener(() =>
			{
				transport.ConnectionData.Address = Address.text;
				transport.ConnectionData.Port = ushort.Parse(Port.text);
				NetworkManager.Singleton.StartClient();

			});
			Host.onClick.AddListener(() =>
			{
				transport.ConnectionData.Address = Address.text;
				transport.ConnectionData.Port = ushort.Parse(Port.text);
				NetworkManager.Singleton.StartHost();
			});
		}

		// Update is called once per frame
		void Update()
		{
		}
	}
}
