using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PDA.Repository;

namespace PDA.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICargoRepository, CargoRepository>();
            services.AddScoped<IPortRepository, PortRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITerminalRepository, TerminalRepository>();
            services.AddScoped<IVesselTypeRepository, VesselTypeRepository>();
            
            services.AddScoped<IPilotageRepository, PilotageRepository>();

            return services;
        }
    }
}
