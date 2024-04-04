using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
namespace LibFPS.Networking
{
    public class Syncer : MonoBehaviour
    {
        public static Syncer Instance;
        public WorkMode CurrentRole;

        public void Start()
        {
            Instance = this;
        }
        public void StartListen()
        {
            Task.Run(() =>
            {
                Socket s=new Socket(AddressFamily.Unknown, SocketType.Stream, ProtocolType.Tcp);

            });
        }
        public void StartSync()
        {
            Task.Run(() =>
            {
            });
        }
        public void Update()
        {

        }
    }
    public enum WorkMode
    {
        Server, Client
    }
}
