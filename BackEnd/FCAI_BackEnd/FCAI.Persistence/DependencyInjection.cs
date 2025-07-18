using FCAI.Domain.Contracts;
using FCAI.Persistence.Data;
using FCAI.Persistence.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FCAI.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection service, IConfiguration configuration)
        {
            // Configure DbContext
            service.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("ApplicationConnection"));
            });

            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<ApplicationInitializer>();
            return service;
        }
    }
}
