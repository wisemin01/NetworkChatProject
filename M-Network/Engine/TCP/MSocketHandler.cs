using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace MNetwork.Engine.TCP
{
    internal class MTCPSocketHandler
    {
        public Socket workSocket = null;

        public const int maxBufferSize = 1024 * 4;

        public byte[] buffer = new byte[maxBufferSize];
    }

}
