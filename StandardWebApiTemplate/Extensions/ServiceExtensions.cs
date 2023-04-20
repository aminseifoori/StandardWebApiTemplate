using Interfaces;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace StandardWebApiTemplate.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin();
                    //builder.WithOrigins("The Required Origins");
                    builder.AllowAnyMethod();
                    //builder.WithMethods();
                    builder.AllowAnyHeader();
                    //builder.WithHeaders();
                });
            });
        }

        public static void ConfigureLoggerManager(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureSQLContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("default"));
            });
        }

    }
}
