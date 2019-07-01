using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using MNetwork.Callback;
using MNetwork.Packet;
using MNetwork.Debuging;
using MNetwork.Logic;

namespace MNetwork.Engine.TCP
{
    public class MTCPNetwork
    {
        public NetworkCallback Callback { get; set; } = null;
        public LogicDispatcher Dispatcher
        {
            set
            {
                MEngine.Instance.Dispatcher = value;
            }
        }

        public bool IsConnected
        {
            get
            {
                return Callback.IsConnected;
            }
        }

        public bool Initialize(string moduleName, NetworkCallback callback)
        {
            if (MEngine.Instance.CreateEngine(false) == false)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), "Failed - MEngine.Instance.CreateEngine() Failed");
                return false;
            }

            Callback = callback;

            return true;
        }

        public bool Start(string IP, ushort port)
        {
            if (MEngine.Instance.Start(IP, port) == false)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), "Failed - MEngine.Instance.Start() Failed");
                return false;
            }

            return true;
        }

        public bool Shutdown()
        {
            return MEngine.Instance.ShutDown();
        }

        public bool Update()
        {
            while (true)
            {
                BasePacket packet = LogicGateWay.PopPacket();

                if (packet == null)
                {
                    break;
                }

                switch (packet.ProcessType)
                {
                    case PacketEnum.ProcessType.Connect:
                        Callback.HandleConnect();
                        break;
                    case PacketEnum.ProcessType.Disconnect:
                        Callback.HandleDisconnect();
                        break;
                    case PacketEnum.ProcessType.Data:
                        Callback.HandleNetworkMessage(packet);
                        break;
                    case PacketEnum.ProcessType.RPC:
                        Callback.HandleRPC(packet);
                        break;
                }
            }

            return true;
        }

        public bool SendRequest(BasePacket packet)
        {
            return MEngine.Instance.SendRequest(packet);
        }

        public BasePacket RPCResult()
        {
            return null;
        }

        public void SetPacketDistinctioner(PacketDistinctioner distinctioner)
        {
            MEngine.Instance.packetDistinctioner = distinctioner;
        }
    }
}
