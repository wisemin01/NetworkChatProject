using System;

namespace MNetwork.Engine.TCP
{
    internal abstract class MTCPNetworkEngine : BaseNetworkEngine
    {
        internal MTCPNetworkEngine(BaseEngine engine) : base(engine) { }
    }
}