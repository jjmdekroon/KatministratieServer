using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using Superkatten.Katministratie.Infrastructure.Mapper;
using Superkatten.Katministratie.Infrastructure.Persistence;

namespace Superkatten.Katministratie.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext.UseSqlite("Data Source=./katministratie.db;", options =>
            //{
            //});

            services.AddTransient<ISuperkattenRepository, SuperkattenRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IUserAuthorisationRepository, UserAuthorisationRepository>();
            services.AddTransient<IMedicalProceduresRepository, MedicalProceduresRepository>();
            services.AddTransient<IReportingRepository, ReportingRepository>();
            services.AddTransient<IUserMapper, UserMapper>();
            services.AddTransient<ICatchOriginRepository, CatchOriginRepository>();

            return services;
        }
    }
}
