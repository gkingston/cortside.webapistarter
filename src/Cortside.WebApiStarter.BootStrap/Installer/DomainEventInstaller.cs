using Cortside.Common.BootStrap;
using Cortside.Common.DomainEvent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.WebApiStarter.BootStrap.Installer {
    class DomainEventInstaller : IInstaller {
        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            var config = configuration.GetSection("ServiceBus");
            var rsettings = new ServiceBusReceiverSettings {
                Address = config.GetValue<string>("Queue"),
                AppName = config.GetValue<string>("AppName"),
                Protocol = config.GetValue<string>("Protocol"),
                PolicyName = config.GetValue<string>("Policy"),
                Key = config.GetValue<string>("Key"),
                Namespace = config.GetValue<string>("Namespace"),
                Durable = 1,
                Credits = config.GetValue<int>("Credits")
            };
            services.AddSingleton(rsettings);

            var psettings = new ServiceBusPublisherSettings {
                Address = config.GetValue<string>("Exchange"),
                AppName = config.GetValue<string>("AppName"),
                Protocol = config.GetValue<string>("Protocol"),
                PolicyName = config.GetValue<string>("Policy"),
                Key = config.GetValue<string>("Key"),
                Namespace = config.GetValue<string>("Namespace"),
                Durable = 1,
                Credits = config.GetValue<int>("Credits")
            };
            services.AddSingleton(psettings);
        }
    }
}
