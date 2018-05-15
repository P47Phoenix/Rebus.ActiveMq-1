using System;
using System.Linq;
using NUnit.Framework;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Tests.Contracts;
using Rebus.ActiveMq.Config;

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
                .Transport(t => t.UseActiveMq)
                .Start();
        }

        [Test]
        public void TestMethod1()
        {
        }
    }
}
