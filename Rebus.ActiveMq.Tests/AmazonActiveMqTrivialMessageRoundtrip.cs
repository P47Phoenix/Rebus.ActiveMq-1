using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Messages;
using Rebus.Tests.Contracts;
using Rebus.Tests.Contracts.Extensions;

#pragma warning disable 1998

namespace Rebus.ActiveMq.Tests
{
    [TestFixture]
    [Category(Category.AmazonSqs)]
    public class AmazonActiveMqTrivialMessageRoundtrip : ActiveMqFixtureBase
    {
        private ActiveMqTransport _transport;
        private string _brilliantQueueName;

        protected override void SetUp()
        {
            _brilliantQueueName = TestConfig.GetName("trivialroundtrippin");
            _transport = ActiveMqTransportFactory.CreateTransport(_brilliantQueueName, TimeSpan.FromSeconds(30));
            _transport.Delete();
            _transport.CreateQueue();
        }

        private static Dictionary<string, string> NewFineHeaders()
        {
            return new Dictionary<string, string> {{Headers.MessageId, Guid.NewGuid().ToString()}};
        }

        [Test]
        public async Task CanRoundtripSingleMessageWithBus()
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                var gotTheMessage = new ManualResetEvent(false);

                activator.Handle<string>(async message => { gotTheMessage.Set(); });

                Configure.With(activator).Transport(t => t.Register(c => _transport)).Start();

                await activator.Bus.SendLocal("HAIIIIIIIIIIIIIIIIII!!!!111");

                gotTheMessage.WaitOrDie(TimeSpan.FromSeconds(5));
            }
        }

        [Test]
        public async Task CanRoundtripSingleMessageWithTransport()
        {
            const string positiveGreeting = "hej meeeeed dig min vennnnn!!!!!!111";

            await WithContext(async context =>
            {
                var message = new TransportMessage(NewFineHeaders(), Encoding.UTF8.GetBytes(positiveGreeting));

                await _transport.Send(_brilliantQueueName, message, context);
            });

            var receivedMessage = await _transport.WaitForNextMessage();

            Assert.That(Encoding.UTF8.GetString(receivedMessage.Body), Is.EqualTo(positiveGreeting));
        }
    }
}
