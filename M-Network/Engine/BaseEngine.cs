using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Logic;
using MNetwork.Packet;

namespace MNetwork.Engine
{
    public abstract class BaseEngine
    {
        public int MaxUserCount { get; protected set; } = 0;
        public LogicDispatcher Dispatcher { get; set; } = null;
        public BaseNetworkEngine NetworkEngine { get; protected set; } = null;
        public PacketDistinctioner packetDistinctioner { get; set; } = null;

        public abstract bool OnConnect(int serial);
        public abstract bool OnDisconnect(int serial);
    }
}
