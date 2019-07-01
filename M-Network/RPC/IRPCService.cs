using MNetwork.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNetwork.RPC
{
    public interface IRPCService
    {
        void ProcessRPCService(BasePacket packet);
    }
}
