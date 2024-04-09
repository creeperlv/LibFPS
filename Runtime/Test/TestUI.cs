using Mirror;
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
        void Start()
        {
            Connect.onClick.AddListener(() =>
            {
                NetworkManager.singleton.networkAddress = Address.text;
                NetworkManager.singleton.StartClient();
            });
            Host.onClick.AddListener(() =>
            {
                NetworkManager.singleton.networkAddress = Address.text;
                NetworkManager.singleton.StartServer();
            });
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
