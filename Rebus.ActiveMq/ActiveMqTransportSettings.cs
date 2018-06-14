namespace Rebus
{
    internal class ActiveMqTransportSettings : IActiveMqTransportSettings
    {
        public string QueueName { get; set; }
        public string BrokerUri { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}