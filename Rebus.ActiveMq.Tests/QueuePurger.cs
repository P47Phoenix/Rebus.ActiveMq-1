using System;
using System.Threading.Tasks;

namespace Rebus.ActiveMq.Tests
{
    /// <summary>
    ///     Purges the queue when it is disposed
    /// </summary>
    internal class QueuePurger : IDisposable
    {
        private readonly string _queueName;

        public QueuePurger(string queueName)
        {
            _queueName = queueName;
        }

        public void Dispose()
        {
            ActiveMqTransportFactory.CreateTransport(_queueName, TimeSpan.FromMinutes(5)).Delete().RunSync();
        }
    }
}
