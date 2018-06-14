using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Tests.Contracts;
using Rebus.Tests.Contracts.Extensions;

#pragma warning disable 1998

namespace Rebus.ActiveMq.Tests
{
    [TestFixture]
    [Category(Category.AmazonSqs)]
    public class DeferMessageTest : ActiveMqFixtureBase
    {
        private BuiltinHandlerActivator _activator;
        private RebusConfigurer _configurer;

        protected override void SetUp()
        {
            var queueName = TestConfig.GetName("defertest");

            AmazonSqsManyMessagesTransportFactory.PurgeQueue(queueName);

            _activator = Using(new BuiltinHandlerActivator());

            _configurer = Configure.With(_activator)
                .Transport(t => t.SetupTestQueue(queueName))
                .Options(o => o.LogPipeline());
        }

        [Test]
        public async Task CanDeferMessage()
        {
            var gotTheMessage = new ManualResetEvent(false);

            var receiveTime = DateTime.MaxValue;

            _activator.Handle<string>(async str =>
            {
                receiveTime = DateTime.UtcNow;
                gotTheMessage.Set();
            });

            var bus = _configurer.Start();
            var sendTime = DateTime.UtcNow;

            await bus.DeferLocal(TimeSpan.FromSeconds(10), "hej med dig!");

            gotTheMessage.WaitOrDie(TimeSpan.FromSeconds(20));

            var elapsed = receiveTime - sendTime;

            Assert.That(elapsed, Is.GreaterThan(TimeSpan.FromSeconds(8)));
        }
    }
}
