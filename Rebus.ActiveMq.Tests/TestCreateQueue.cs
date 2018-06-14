using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Tests.Contracts;

namespace Rebus.ActiveMq.Tests
{
    [TestFixture, Category("activemq")]
    public class TestCreateQueue : FixtureBase
    {

        IBus _bus;

        protected override void SetUp()
        {
            var client = Using(new BuiltinHandlerActivator());
            
            _bus = Configure.With(client)
                .Logging(l => l.Console(minLevel: LogLevel.Warn))
                .Transport(t => t.SetupTestQueue(nameof(TestCreateQueue)))
                .Start();
        }

        

        [Test]
        public async Task CreateQueue()
        {
            await _bus.Send(new TestMessage());
        }
    }


    public class TestMessage
    {
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
