using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using System.Net;
using System.Net.Sockets;
using System.Threading;

using MNetwork.Debuging;
using MNetwork.Packet;

namespace MNetwork.Engine.TCP
{
    internal class MTCPClientEngine : MTCPNetworkEngine
    {
        private Socket clientSocket = null;
        private MTCPSocketHandler handler = null;

        public MTCPClientEngine(BaseEngine engine) : base(engine) { }

        public override bool Disconnect(int serial)
        {
            return true;
        }

        public override bool Initialize()
        {
            clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            return true;
        }

        public override bool SendRequest(BasePacket packet)
        {
            packet.Encode();

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();

            args.SetBuffer(handler.buffer, 0, handler.buffer.Length);
            args.Completed += OnSendComplate;
            args.UserToken = handler;

            clientSocket.SendAsync(args);

            return true;
        }

        public override bool Shutdown()
        {
            clientSocket.Close();
            return true;
        }

        public override bool Start(string IP, ushort port)
        {
            Debug.Log($"====================================================");
            Debug.Log($" * Client Start [IP : {IP}] [PORT : {port}] * ");
            Debug.Log($"====================================================");

            return AddConnector(1, IP, port) >= 0;
        }

        public override int AddConnector(int connectorId, string IP, ushort port)
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(IP), port);

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += OnConnectComplate;
                args.RemoteEndPoint = ipep;

                clientSocket.ConnectAsync(args);
            }
            catch (Exception e)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), e.Message);
                return -1;
            }
            return connectorId;
        }

        private void OnConnectComplate(object sender, SocketAsyncEventArgs e)
        {
            Socket client = sender as Socket;

            if (client.Connected)
            {
                handler = new MTCPSocketHandler();

                SocketAsyncEventArgs args = new SocketAsyncEventArgs
                {
                    UserToken = handler
                };

                args.SetBuffer(handler.buffer, 0, handler.buffer.Length);
                args.Completed += OnReceiveComplate;

                client.ReceiveAsync(args);
            }
            else
            {
                client = null;
            }
        }

        private void OnReceiveComplate(object sender, SocketAsyncEventArgs e)
        {
            Socket client = sender as Socket;

            if (client.Connected && e.BytesTransferred > 0)
            {
                BasePacket packet = defaultEngine.packetDistinctioner.Distinction(e.Buffer);
                defaultEngine.Dispatcher.Dispatch(packet);

                client.ReceiveAsync(e);
            }
            else
            {
                client = null;
            }
        }

        private void OnSendComplate(object sender, SocketAsyncEventArgs e)
        {

        }
    }
}
