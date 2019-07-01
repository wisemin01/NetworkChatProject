using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;

namespace MNetwork.Packet
{
    public class ProtobufPacket<T> : ProtobufPacketImpl
        where T : class, IMessage<T>, new()
    {
        public ProtobufPacket() : base(null)
        {
            message = new T();
        }

        public ProtobufPacket(byte[] buffer) : base(buffer)
        {
            message = new T();
            Decode(buffer);
        }

        public ProtobufPacket(T protobufPacket) : base(null)
        {
            message = protobufPacket;
            Encode();
        }

        public ProtobufPacket(int serial, PacketEnum.ProcessType processType, int type, T protobufPacket) : base(serial, processType, type)
        {
            message = protobufPacket;
            Encode();
        }

        public override BasePacket Clone()
        {
            return base.Clone();
        }

        public T ProtobufMessage
        {
            get { return message as T; }
        }
    }
}
