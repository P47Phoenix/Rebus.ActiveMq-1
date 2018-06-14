using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Rebus.Exceptions;
using Rebus.Extensions;
using Rebus.Logging;
using Rebus.Tests.Contracts.Transports;
using Rebus.Threading.TaskParallelLibrary;
using Rebus.Transport;

namespace Rebus.ActiveMq.Tests
{
    internal class ActiveMqTransportFactory : ITransportFactory
    {
        private readonly ConcurrentDictionary<string, ActiveMqTransport> _queuesToDelete = new ConcurrentDictionary<string, ActiveMqTransport>();

        public ITransport CreateOneWayClient()
        {
            return Create(null, TimeSpan.FromSeconds(30));
        }

        public ITransport Create(string inputQueueAddress)
        {
            return Create(inputQueueAddress, TimeSpan.FromSeconds(30));
        }

        internal static ActiveMqTransport CreateTransport(string brilliantQueueName, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        public void CleanUp()
        {
            CleanUp(false);
        }

        public ITransport Create(string inputQueueAddress, TimeSpan peeklockDuration, IActiveMqTransportSettings options = null)
        {
            return inputQueueAddress == null ? CreateTransport(null, options) : _queuesToDelete.GetOrAdd(inputQueueAddress, k => CreateTransport(inputQueueAddress, options));
        }

        public static ActiveMqTransport CreateTransport(string inputQueueAddress, IActiveMqTransportSettings options = null)
        {
            var activeMqTransportSettings = options ?? new ActiveMqTransportSettings();

            activeMqTransportSettings.SetActiveMqTransportSettings(inputQueueAddress);

            var consoleLoggerFactory = new ConsoleLoggerFactory(false);

            var transport = new ActiveMqTransport(activeMqTransportSettings);

            return transport;
        }

        public void CleanUp(bool deleteQueues)
        {
            if (deleteQueues == false)
            {
                return;
            }

            foreach (var queueAndTransport in _queuesToDelete)
            {
                var transport = queueAndTransport.Value;

                transport.Delete().RunSync();
            }
        }
    }
}
