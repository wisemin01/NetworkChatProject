using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Packet;

namespace MNetwork.Logic
{
    internal class LogicHandler
    {
        private static LogicEntry logicEntry = null;

        public static bool Initialize(LogicEntry logic)
        {
            logicEntry = logic;

            if (logicEntry.Initialize() == false)
                return false;

            return true;
        }

        public static bool ProcessPacket(BasePacket packet)
        {
            if (packet.IsConnectorProcess)
            {
                logicEntry.ProcessConnectorPacket(packet);
            }
            else
            {
                logicEntry.ProcessPacket(packet);
            }

            return true;
        }

        public static bool DestroyLogic()
        {
            if (logicEntry == null)
            {
                return false;
            }

            logicEntry.Dispose();
            logicEntry = null;

            return true;
        }

        public static void SetLogic(LogicEntry logic)
        {
            logicEntry = logic;
        }
    }
}
