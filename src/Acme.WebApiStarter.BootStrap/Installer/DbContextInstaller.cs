using System;
using Cortside.Common.BootStrap;
using Acme.WebApiStarter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.WebApiStarter.BootStrap.Installer {
    public class DbContextInstaller : IInstaller {
        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            services.AddDbContext<DatabaseContext>(opt => {
                opt.UseSqlServer(configuration.GetSection("WebApiStarter").GetValue<string>("ConnectionString"),
                    sqlServerOptionsAction: sqlOptions => {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 2,
                            maxRetryDelay: TimeSpan.FromSeconds(1),
                            errorNumbersToAdd: null);
                    });
                opt.EnableSensitiveDataLogging();
            });
            services.AddScoped<DatabaseContext, DatabaseContext>();

            // for DbContextCheck
            services.AddTransient<DbContext, DatabaseContext>();
        }
    }
}
