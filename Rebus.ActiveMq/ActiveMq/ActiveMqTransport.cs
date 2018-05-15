﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.Util;
using Rebus.Messages;
using Rebus.Transport;

namespace Rebus.ActiveMq
{
    internal class ActiveMqTransport : ITransport
    {
        private readonly IActiveMqTransportSettings _activeMqTransportSettings;

        private readonly IActiveMqSessionHelper _activeMqSessionHelper;

        public ActiveMqTransport(IActiveMqTransportSettings activeMqTransportSettings)
        {
            _activeMqTransportSettings = activeMqTransportSettings;
            _activeMqSessionHelper = new ActiveMqSessionHelper(activeMqTransportSettings);
        }

        public string Address => _activeMqTransportSettings.QueueName;

        public void CreateQueue(string address)
        {
            var task = _activeMqSessionHelper.UseSession(async session =>
            {
                using (var queue = SessionUtil.GetQueue(session.Session, Address))
                {
                }
            });

            AsyncHelpers.RunSync(() => task);
        }

        public async Task<TransportMessage> Receive(ITransactionContext context, CancellationToken cancellationToken)
        {
            return await _activeMqSessionHelper.UseSession(async session =>
            {
                using (var consumer = session.CreateQueueMessageConsumer(Address))
                {
                    var message = consumer.Receive();

                    var headers = new Dictionary<string, string>();

                    foreach (var key in consumer.Receive().Properties.Keys.Cast<string>())
                    {
                        var value = (string)message.Properties[key];

                        headers.Add(key, value);
                    }

#if NET45
                    byte[] body = message.ToObject<byte[]>();
#elif NETCOREAPP2_0
                    byte[] body = message.ToObject<byte[]>();
#endif

                    return new TransportMessage(headers, body);
                }
            });
        }

        public async Task Send(string destinationAddress, TransportMessage message, ITransactionContext context)
        {
            await _activeMqSessionHelper.UseSession(async session =>
            {
                var producer = session.Session.CreateProducer(new ActiveMQQueue(Address));

                var msg = producer.CreateBytesMessage(message.Body);
                msg.NMSDeliveryMode = MsgDeliveryMode.Persistent;
                msg.NMSMessageId = message.Headers[Headers.MessageId];
                msg.NMSMessageId = message.Headers[Headers.CorrelationId];

                foreach (var messageHeader in message.Headers)
                {
                    msg.Properties.SetString(messageHeader.Key, messageHeader.Value);
                }
                
                producer.Send(msg);
            });
        }
    }

}
