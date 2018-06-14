using NUnit.Framework;
using Rebus.Tests.Contracts.Transports;

namespace Rebus.ActiveMq.Tests
{
    [TestFixture]
    [Category(Category.AmazonSqs)]
    internal class AmazonSqsSimpleSend : BasicSendReceive<ActiveMqTransportFactory>
    {
    }
}
