using MNetwork.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNetwork.Logic
{
    class LogicGateWay
    {
        private static Queue<BasePacket> packetQueue = new Queue<BasePacket>();

        public static bool PushPacket(BasePacket packet)
        {
            try
            {
                packetQueue.Enqueue(packet);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public static BasePacket PopPacket()
        {
            if (packetQueue.Count == 0)
                return null;
            else
                return packetQueue.Dequeue();
        }
    }
}
