using System;
using Rebus.Transport;

namespace Rebus.Config
{
    public static class ActiveMqConfigurationExtensions
    {
        public static void UseActiveMq(this StandardConfigurer<ITransport> configurer, Action<IActiveMqTransportSettings> activeMqTransportSettingsFunc)
        {
            configurer
                .OtherService<IActiveMqTransportSettings>()
                .Register(c =>
                {
                    IActiveMqTransportSettings settings = new ActiveMqTransportSettings();
                    activeMqTransportSettingsFunc(settings);
                    return settings;
                });
            configurer
                .Register(context =>
                {
                    var settings = context.Get<IActiveMqTransportSettings>();
                    return new ActiveMqTransport(settings);
                });
        }
    }
}
