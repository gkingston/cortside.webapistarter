using System.Linq;
using System.Reflection;
using Cortside.Common.BootStrap;
using Acme.WebApiStarter.DomainService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.WebApiStarter.BootStrap.Installer {
    public class DomainInstaller : IInstaller {
        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            // register all services, handlers and factories
            Assembly.GetEntryAssembly().GetTypes()
                .Where(x => (x.Name.EndsWith("Service") || x.Name.EndsWith("Handler") || x.Name.EndsWith("Factory") || x.Name.EndsWith("Client"))
                    && x.GetTypeInfo().IsClass
                    && !x.GetTypeInfo().IsAbstract
                    && x.GetInterfaces().Any())
                .ToList().ForEach(x => {
                    x.GetInterfaces().ToList()
                        .ForEach(i => services.AddSingleton(i, x));
                });

            // register domain services
            typeof(SubjectService).GetTypeInfo().Assembly.GetTypes()
                .Where(x => (x.Name.EndsWith("Service"))
                    && x.GetTypeInfo().IsClass
                    && !x.GetTypeInfo().IsAbstract
                    && x.GetInterfaces().Any())
                .ToList().ForEach(x => {
                    x.GetInterfaces().ToList()
                        .ForEach(i => services.AddSingleton(i, x));
                });
        }
    }
}
