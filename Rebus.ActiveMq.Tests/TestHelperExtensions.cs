using System;
using System.Threading.Tasks;
using Rebus.Config;
using Rebus.Transport;

namespace Rebus.ActiveMq.Tests
{
    public static class TestHelperExtensions
    {
        public static void SetupTestQueue(this StandardConfigurer<ITransport> transport, string queueName)
        {
            transport.UseActiveMq(s => s.SetActiveMqTransportSettings(queueName));
        }

        public static void SetActiveMqTransportSettings(this IActiveMqTransportSettings settings, string queueName)
        {
            settings.BrokerUri = "http://localhost:8161";
            settings.Password = "your_password";
            settings.QueueName = queueName;
            settings.UserName = "admin";
        }

        public static void RunSync(this Task task)
        {
            AsyncHelpers.RunSync(() => task);
        }
    }
}
