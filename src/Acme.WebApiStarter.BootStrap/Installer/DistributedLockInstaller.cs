using Cortside.Common.BootStrap;
using Medallion.Threading;
using Medallion.Threading.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.WebApiStarter.BootStrap.Installer {
    public class DistributedLockInstaller : IInstaller {
        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            var connectionString = configuration.GetSection("WebApiStarter").GetValue<string>("ConnectionString");
            IDistributedLockProvider provider = new SqlDistributedSynchronizationProvider(connectionString);
            services.AddSingleton(provider);
        }
    }
}
