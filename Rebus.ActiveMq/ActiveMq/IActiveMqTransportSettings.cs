namespace Rebus.ActiveMq
{

    public interface IActiveMqTransportSettings
    {
        string QueueName { get; set; }
        string BrokerUri { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}