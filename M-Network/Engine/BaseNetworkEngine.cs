using MNetwork.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace MNetwork.Engine
{
    public abstract class BaseNetworkEngine
    {
        protected BaseEngine defaultEngine = null;

        public BaseNetworkEngine(BaseEngine engine)
        {
            defaultEngine = engine;
        }

        public abstract bool Initialize();
        public abstract bool Start(string IP, ushort port);

        public abstract bool Shutdown();
        public abstract bool SendRequest(BasePacket packet);

        public abstract bool Disconnect(int serial);

        public virtual int AddConnector(int connectorId, string IP, ushort port) { return -1; }
        
        public virtual int AddListener(string IP, ushort port, bool defaultListener = false) { return -1; }
    }
}
