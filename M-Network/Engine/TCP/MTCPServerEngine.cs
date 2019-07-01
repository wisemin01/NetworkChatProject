using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

using MNetwork.Debuging;
using MNetwork.Packet;

namespace MNetwork.Engine.TCP
{
    // MTCPServerEngine Begin() - End() Version
    /*
    internal class MTCPServerEngine : MTCPNetworkEngine
    {
        private ManualResetEvent manualEvent = new ManualResetEvent(false);
        private TcpListener listener = null;

        private Dictionary<int, MSocketHandler> socketHandlers = new Dictionary<int, MSocketHandler>();

        private bool IsLoopEnable = false;

        public int MaxAcceptCount { get; set; } = 128;

        public MTCPServerEngine(BaseEngine engine) : base(engine) { }

        public override bool Initialize()
        {
            return true;
        }

        public override bool Start(string IP, ushort port)
        {
            Debug.Log($"====================================================");
            Debug.Log($" * Server Start [IP : {IP}] [PORT : {port}] * ");
            Debug.Log($"====================================================");

            try
            {
                listener = new TcpListener(IPAddress.Parse(IP), port);
                Debug.Log(MethodBase.GetCurrentMethod(), "Create TcpListener");
            }
            catch (Exception e)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), e.Message);
                return false;
            }

            IsLoopEnable = true;

            Thread AcceptThread = new Thread(MainLoop) { IsBackground = true };
            AcceptThread.Start();

            return true;
        }

        public override bool Shutdown()
        {
            Debug.Log($"====================================================");
            Debug.Log($" * Server Shutdown * ");
            Debug.Log($"====================================================");

            listener.Stop();
            IsLoopEnable = false;

            return true;
        }

        // 패킷을 보냅니다.
        public override bool SendRequest(BasePacket packet)
        {
            packet.Encode();

            if (socketHandlers.TryGetValue(packet.Serial, out MSocketHandler handler) == false)
            {
                return false;
            }

            Send(handler.workSocket, packet.Buffer);

            return true;
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                manualEvent.Set();

                TcpListener listener = (TcpListener)ar.AsyncState;
                Socket handler = listener.EndAcceptSocket(ar);

                int socketID = handler.Handle.ToInt32();

                Debug.Log(MethodBase.GetCurrentMethod(), $"Accept Socket [SOCKET ID : {socketID}]");

                defaultEngine.OnConnect(socketID);

                MSocketHandler socketHandler = new MSocketHandler
                {
                    workSocket = handler
                };

                handler.BeginReceive(socketHandler.buffer,
                    0, MSocketHandler.maxBufferSize,
                    0, new AsyncCallback(RecvCallback),
                    socketHandler);

                socketHandlers.Add(socketID, socketHandler);
            }
            catch (Exception e)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), e.Message);
            }
        }

        public void RecvCallback(IAsyncResult ar)
        {
            try
            {
                MSocketHandler handler = (MSocketHandler)ar.AsyncState;

                handler.workSocket.EndReceive(ar);
                int socketID = handler.workSocket.Handle.ToInt32();

                Debug.Log(MethodBase.GetCurrentMethod(), $"Read Packet [SOCKET ID : {socketID}]");

                BasePacket packet = defaultEngine.packetDistinctioner.Distinction(handler.buffer);

                defaultEngine.Dispatcher.Dispatch(packet);
            }
            catch (Exception e)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), e.Message);
            }
        }

        public void Send(Socket handler, byte[] buffer)
        {
            handler.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int socketID = handler.Handle.ToInt32();

                Debug.Log(MethodBase.GetCurrentMethod(), $"Send Packet [SOCKET ID : {socketID}]");
                int bytes = handler.EndSend(ar);
            }
            catch (Exception e)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), e.Message);
            }
        }

        public override bool Disconnect(int serial)
        {
            if (socketHandlers.TryGetValue(serial, out MSocketHandler handler) == false)
            {
                return false;
            }

            handler.workSocket.Close();

            socketHandlers.Remove(serial);

            return true;
        }

        private void MainLoop()
        {
            Debug.Log(MethodBase.GetCurrentMethod(), "Starting Accept Loop");

            try
            {
                listener.Start(MaxAcceptCount);

                while (IsLoopEnable)
                {
                    manualEvent.Reset();

                    listener.BeginAcceptSocket(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    manualEvent.WaitOne();
                }

            }
            catch (Exception e)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), e.Message);
            }
        }
    }
    */

    internal class MTCPServerEngine : MTCPNetworkEngine
    {
        private Dictionary<int, MTCPSocketHandler> socketHandlers = new Dictionary<int, MTCPSocketHandler>();
        private Socket listener;

        private const int maxListenCount = 64;

        public MTCPServerEngine(BaseEngine engine) : base(engine)
        {

        }

        public override bool Disconnect(int serial)
        {
            if (socketHandlers.TryGetValue(serial, out MTCPSocketHandler handler) == false)
            {
                return false;
            }

            handler.workSocket.Disconnect(false);
            handler.workSocket.Dispose();

            socketHandlers.Remove(serial);

            return true;
        }

        public override bool Initialize()
        {
            return true;
        }

        public override bool SendRequest(BasePacket packet)
        {
            packet.Encode();

            if (socketHandlers.TryGetValue(packet.Serial, out MTCPSocketHandler handler) == false)
            {
                return false;
            }

            Send(handler.workSocket, packet.Buffer);

            return true;
        }

        public override bool Shutdown()
        {
            Debug.Log($"====================================================");
            Debug.Log($" * Server Shutdown * ");
            Debug.Log($"====================================================");

            return true;
        }

        public void Send(Socket client, byte[] buffer)
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();

            args.SetBuffer(buffer, 0, buffer.Length);
            args.Completed += OnSendComplate;

            client.SendAsync(args);
        }

        public override bool Start(string IP, ushort port)
        {
            try
            {
                Debug.Log($"====================================================");
                Debug.Log($" * Server Start [IP : {IP}] [PORT : {port}] * ");
                Debug.Log($"====================================================");

                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(IP), port);

                listener.Bind(ipep);
                listener.Listen(maxListenCount);

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += OnAcceptComplate;
                listener.AcceptAsync(args);
            }
            catch (Exception e)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), e.Message);
                return false;
            }

            return true;
        }

        public void OnAcceptComplate(object sender, SocketAsyncEventArgs e)
        {
            if (e.AcceptSocket == null)
                return;

            MTCPSocketHandler clientHandler = new MTCPSocketHandler
            {
                workSocket = e.AcceptSocket
            };

            int serial = clientHandler.workSocket.Handle.ToInt32();

            socketHandlers.Add(serial, clientHandler);
            defaultEngine.OnConnect(serial);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();

            args.SetBuffer(clientHandler.buffer, 0, MTCPSocketHandler.maxBufferSize);
            args.UserToken = clientHandler;
            args.Completed += OnReceiveComplate;

            clientHandler.workSocket.ReceiveAsync(args);

            e.AcceptSocket = null;
            listener.AcceptAsync(e);
        }

        public void OnReceiveComplate(object sender, SocketAsyncEventArgs e)
        {
            Socket client = (Socket)sender;
            MTCPSocketHandler handler = (MTCPSocketHandler)e.UserToken;

            if (client.Connected && e.BytesTransferred > 0)
            {
                // Success
                BasePacket receiveDataPacket = defaultEngine.packetDistinctioner.Distinction(handler.buffer);

                receiveDataPacket.Serial = client.Handle.ToInt32();

                defaultEngine.Dispatcher.Dispatch(receiveDataPacket);

                client.ReceiveAsync(e);
            }
            else
            {
                // Fail
                client.Disconnect(false);
                client.Dispose();

                int serial = client.Handle.ToInt32();
                socketHandlers.Remove(serial);
                defaultEngine.OnDisconnect(serial);
            }
        }

        public void OnSendComplate(object sender, SocketAsyncEventArgs e)
        {
        }
    }
}
