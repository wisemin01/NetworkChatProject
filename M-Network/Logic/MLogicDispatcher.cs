using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using MNetwork.Packet;
using MNetwork.RPC;
using MNetwork.Engine;

namespace MNetwork.Logic
{
    public class MLogicDispatcher : LogicDispatcher
    {
        public bool IsLogicEnd { get; set; } = false;

        public IRPCService RPCService { protected get; set; } = null;

        public override bool CreateLogicSystem(LogicEntry logicEntry)
        {
            if (LogicHandler.Initialize(logicEntry) == false)
                return false;

            IsLogicEnd = false;

            // LogicHandling Thread
            new Thread(LogicThreadProcess)
            {
                IsBackground = true
            }.Start();

            // RPCHandlingThread
            new Thread(RPCThreadProcess)
            {
                IsBackground = true
            }.Start();
            
            return true;
        }

        public override void Dispatch(BasePacket packet)
        {
            if (packet.ProcessType == PacketEnum.ProcessType.RPC
                && MEngine.Instance.IsServer == true)
            {
                // RPC Handling
                RPCGateWay.PushPacket(packet);
            }
            else
            {
                // Logic Handling
                LogicGateWay.PushPacket(packet);
            }
        }

        public override bool ShutDownLogicSystem()
        {
            IsLogicEnd = true;

            LogicHandler.DestroyLogic();

            return true;
        }

        public void LogicThreadProcess()
        {
            while (IsLogicEnd == false)
            {
                BasePacket packet = LogicGateWay.PopPacket();

                if (packet != null)
                {
                    LogicHandler.ProcessPacket(packet);
                }
            }
        }

        public void RPCThreadProcess()
        {
            while (IsLogicEnd == false)
            {
                BasePacket packet = RPCGateWay.PopPacket();

                if (packet != null)
                {
                    RPCService.ProcessRPCService(packet);
                }
            }
        }


    }
}
