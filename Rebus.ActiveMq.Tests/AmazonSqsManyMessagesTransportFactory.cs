using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Tests.Contracts.Transports;
using Rebus.Threading.TaskParallelLibrary;

namespace Rebus.ActiveMq.Tests
{
    public class AmazonSqsManyMessagesTransportFactory : IBusFactory
    {
        private readonly List<IDisposable> _stuffToDispose = new List<IDisposable>();

        public IBus GetBus<TMessage>(string inputQueueAddress, Func<TMessage, Task> handler)
        {
            var builtinHandlerActivator = new BuiltinHandlerActivator();

            builtinHandlerActivator.Handle(handler);

            PurgeQueue(inputQueueAddress);

            var bus = Configure.With(builtinHandlerActivator).Transport(t => t.SetupTestQueue(queueName: inputQueueAddress)).Options(o =>
            {
                o.SetNumberOfWorkers(10);
                o.SetMaxParallelism(10);
            }).Start();

            _stuffToDispose.Add(bus);

            return bus;
        }

        public void Cleanup()
        {
            _stuffToDispose.ForEach(d => d.Dispose());
            _stuffToDispose.Clear();
        }

        public static void PurgeQueue(string queueName)
        {
            var consoleLoggerFactory = new ConsoleLoggerFactory(false);

            var transport = ActiveMqTransportFactory.CreateTransport(queueName);

            transport.Delete().RunSync();
            transport.CreateQueue();
        }
    }
}
