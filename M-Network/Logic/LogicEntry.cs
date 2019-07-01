using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Packet;
using MNetwork.Callback;

namespace MNetwork.Logic
{
    public abstract class LogicEntry : IDisposable
    {
        private Dictionary<int, NetworkCallback> networkCallbacks
             = new Dictionary<int, NetworkCallback>();

        public abstract bool Initialize();
        public abstract void ProcessPacket(BasePacket packet);
        public virtual bool ProcessConnectorPacket(BasePacket packet)
        {
            NetworkCallback callback;

            if (networkCallbacks.TryGetValue(packet.Serial, out callback) == false)
                return false;

            PacketEnum.ProcessType processType = (PacketEnum.ProcessType)packet.ProcessType;

            switch (processType)
            {
                case PacketEnum.ProcessType.Connect:
                    callback.HandleConnect();
                    break;
                case PacketEnum.ProcessType.Disconnect:
                    callback.HandleDisconnect();
                    break;
                case PacketEnum.ProcessType.Data:
                    callback.HandleNetworkMessage(packet);
                    break;
                default:
                    return false;
            }

            return true;
        }

        public abstract LogicEntry Clone();

        public virtual bool AddCallback(int id, NetworkCallback networkCallback)
        {
            if (networkCallbacks.ContainsKey(id))
            {
                return false;
            }

            networkCallbacks.Add(id, networkCallback);
            return true;
        }

        public virtual void Dispose()
        {

        }
    }
}
