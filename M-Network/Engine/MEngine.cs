using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Net;
using System;

using MNetwork.Logic;
using MNetwork.Packet;
using MNetwork.Debuging;

using MNetwork.Engine.TCP;

namespace MNetwork.Engine
{
    public class MEngine : BaseEngine
    {
        public bool IsServer { get; private set; } = false;

        public bool Start(ushort port)
        {
            if (NetworkEngine.Start("0.0.0.0", port) == false)
            {
                return false;
            }

            return true;
        }

        public bool Start(string IP, ushort port)
        {
            if (NetworkEngine.Start(IP, port) == false)
            {
                return false;
            }

            return true;
        }

        public bool Start(IPAddress IP, ushort port)
        {
            if (NetworkEngine.Start(IP.ToString(), port) == false)
            {
                return false;
            }

            return true;
        }

        public bool Intialize(LogicEntry logicEntry, PacketDistinctioner distinctioner, LogicDispatcher dispatcher = null)
        {
            if (logicEntry == null)
            {
                throw new NullReferenceException("MEngine Initialize Failed - logicEntry is null.");
            }

            if (dispatcher == null)
            {
                dispatcher = new MLogicDispatcher();
            }

            if (distinctioner == null)
            {
                throw new NullReferenceException("MEngine Initialize Failed - distinctioner is null.");
            }

            if (NetworkEngine == null)
            {
                CreateEngine(true);
            }

            Dispatcher = dispatcher;
            packetDistinctioner = distinctioner;

            if (Dispatcher.CreateLogicSystem(logicEntry) == false)
            {
                throw new Exceptions.InitializeException("MEngine Initialize Failed - Dispatcher.CreateLogicSystem() Failed");
            }

            if (NetworkEngine.Initialize() == false)
            {
                throw new Exceptions.InitializeException("MEngine Initialize Failed - NetworkEngine.Initialize() Failed");
            }

            return true;
        }

        public bool ShutDown()
        {
            if (Dispatcher != null)
            {
                Dispatcher.ShutDownLogicSystem();
            }

            if (NetworkEngine == null || NetworkEngine.Shutdown() == false)
            {
                return false;
            }

            return true;
        }

        public override bool OnConnect(int serial)
        {
            BasePacket packet = new BasePacket(serial, PacketEnum.ProcessType.Connect);

            Dispatcher.Dispatch(packet);

            return true;
        }

        public override bool OnDisconnect(int serial)
        {
            BasePacket packet = new BasePacket(serial, PacketEnum.ProcessType.Disconnect);

            Dispatcher.Dispatch(packet);

            return true;
        }

        public bool SendRequest(BasePacket packet)
        {
            if (NetworkEngine == null)
            {
                return false;
            }

            NetworkEngine.SendRequest(packet);

            return true;
        }

        public bool SendRequest(BasePacket packet, int[] serialArr)
        {
            if (NetworkEngine == null)
            {
                return false;
            }

            foreach (int serial in serialArr)
            {
                packet.Serial = serial;
                NetworkEngine.SendRequest(packet);
            }

            return true;
        }

        public bool Disconnect(int serial)
        {
            if (NetworkEngine == null)
            {
                return false;
            }

            if (NetworkEngine.Disconnect(serial) == false)
            {
                return false;
            }

            return true;
        }

        public void SendToLogic(BasePacket message)
        {
            LogicGateWay.PushPacket(message);
        }

        public bool CreateEngine(bool server = false)
        {
            IsServer = server;

            if (IsServer)
                NetworkEngine = new MTCPServerEngine(this);
            else
                NetworkEngine = new MTCPClientEngine(this);

            if (NetworkEngine.Initialize() == false)
            {
                Debug.ErrorLog(MethodBase.GetCurrentMethod(), "Failed - NetworkEngine.Initialize Failed");
                return false;
            }

            return true;
        }

        /*
         * Singleton
         */

        private static MEngine instance = null;
        public static MEngine Instance {
            get
            {
                if (instance == null)
                    instance = new MEngine();
                return instance;
            }
        }

    }
}
