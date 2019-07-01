using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;

namespace MNetwork.Packet
{
    
    public class BasePacket
    {
        public int Serial { get; set; } = -1;
        public PacketEnum.ProcessType ProcessType { get; set; } = PacketEnum.ProcessType.None;
        public int Type { get; set; } = -1;
        public byte[] Buffer { get; protected set; } = null;
        public bool IsConnectorProcess { get; set; } = false;

        public BasePacket() { }
        public BasePacket(int serial, PacketEnum.ProcessType processType, int type = -1)
        {
            Serial = serial;
            ProcessType = processType;
            Type = type;
        }

        public virtual BasePacket Clone()
        {
            BasePacket clone = new BasePacket()
            {
                Buffer = new byte[Buffer.Length],
                ProcessType = ProcessType,
                Serial = Serial,
                Type = Type
            };

            Buffer.CopyTo(clone.Buffer, 0);

            return clone;
        }

        public virtual bool Encode()
        {
            return false;
        }

        public virtual bool Decode(byte[] buffer)
        {
            return false;
        }
    }
}