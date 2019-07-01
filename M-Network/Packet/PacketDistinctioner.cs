using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;

namespace MNetwork.Packet
{
    public abstract class PacketDistinctioner
    {
        public abstract BasePacket Distinction(byte[] buffer);
    }
}
