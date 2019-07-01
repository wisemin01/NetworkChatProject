using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Callback;
using MNetwork.Packet;

namespace ClientHost
{
    internal class ChattingCallback : NetworkCallback
    {
        public override bool HandleNetworkMessage(BasePacket packet)
        {
            return true;
        }
    }
}
