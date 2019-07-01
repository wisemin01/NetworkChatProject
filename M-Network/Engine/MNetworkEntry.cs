using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using MNetwork.Callback;
using MNetwork.Logic;
using MNetwork.Debuging;
using MNetwork.Packet;

using MNetwork.Engine.TCP;

namespace MNetwork.Engine
{
    public class MNetworkEntry
    {
        public MTCPNetwork TCPNetwork { get; private set; } = null;

        public bool IsConnected { get => TCPNetwork.IsConnected; }

        public bool Initialize(NetworkCallback callback, PacketDistinctioner distinctioner, LogicDispatcher dispatcher = null)
        {
            if (callback == null)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), "Failed - callback is null");
                return false;
            }

            if (distinctioner == null)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), "Failed - distinctioner is null");
                return false;
            }

            if (dispatcher == null)
            {
                dispatcher = new MLogicDispatcher();
            }

            TCPNetwork = new MTCPNetwork();

            if (TCPNetwork.Initialize(string.Empty, callback) == false)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), "Failed - MTCPNetwork.Initialize() Failed");
            }

            TCPNetwork.SetPacketDistinctioner(distinctioner);

            return true;
        }

        public bool Shutdown()
        {
            TCPNetwork.Shutdown();
            TCPNetwork = null;

            return true;
        }

        public bool Run(string IP, ushort port)
        {
            if (TCPNetwork.Start(IP, port) == false)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), "Failed = TCPNetwork.Start() Failed");
                return false;
            }

            return true;
        }

        public bool Update()
        {
            if (TCPNetwork == null)
                return false;

            return TCPNetwork.Update();
        }

        public bool Send(BasePacket packet)
        {
            if (TCPNetwork == null)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), "Failed - TCPNetwork is null");
                return false;
            }

            return TCPNetwork.SendRequest(packet);
        }

        private static MNetworkEntry instance = null;
        public static MNetworkEntry Instance
        {
            get
            {
                if (instance == null)
                    instance = new MNetworkEntry();
                return instance;
            }
        }
    }
}
