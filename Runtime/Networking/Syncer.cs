using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
namespace LibFPS.Networking
{
    public class Syncer : MonoBehaviour
    {
        public static Syncer Instance;
        public WorkMode CurrentRole;
        public string Address;
        public int Port;
        public Action<Exception> OnConnectionFailed;
        public List<SyncedRoot> Roots;
        public System.Random random;
        public ulong Iteration = 0;
        public void Start()
        {
            Instance = this;
        }
        public void StartListen()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Socket s = new Socket(AddressFamily.Unknown, SocketType.Stream, ProtocolType.Tcp);
                    s.Accept();
                }
            });
        }
        public void StartSync()
        {
            try
            {
                Socket s = new Socket(SocketType.Stream, ProtocolType.Tcp);
                s.Connect(new DnsEndPoint(Address, Port));
                Task.Run(() =>
                {
                });
            }
            catch (Exception e)
            {
                OnConnectionFailed?.Invoke(e);
            }
        }
        void CollectPacket()
        {

        }
        void Sync(Socket s)
        {
            foreach (var item in Roots)
            {

            }
        }
        public void Update()
        {

        }
    }
    public enum Command : byte
    {
        SYNC, CREATE, DESTORY, HANDSHAKE
    }
    public enum WorkMode
    {
        Server, Client
    }
}
