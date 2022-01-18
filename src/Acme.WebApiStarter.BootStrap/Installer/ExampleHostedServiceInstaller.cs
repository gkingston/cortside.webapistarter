using Cortside.Common.BootStrap;
using Acme.WebApiStarter.Configuration;
using Acme.WebApiStarter.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.WebApiStarter.BootStrap.Installer {
    public class ExampleHostedServiceInstaller : IInstaller {
        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            services.AddSingleton(configuration.GetSection("ExampleHostedService").Get<ExampleHostedServiceConfiguration>());
            services.AddHostedService<ExampleHostedService>();
        }
    }
}
