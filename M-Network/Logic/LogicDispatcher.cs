using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Packet;
using MNetwork.RPC;

namespace MNetwork.Logic
{
    public abstract class LogicDispatcher
    {
        // Decoding 된 패킷을 처리합니다.
        public abstract void Dispatch(BasePacket packet);

        public abstract bool CreateLogicSystem(LogicEntry logicEntry);
        public abstract bool ShutDownLogicSystem();

	    public virtual bool  AddRPCService(IRPCService RPCService) { return false; }
    }
}
