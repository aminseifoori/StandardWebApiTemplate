using Interfaces;
using LoggerService;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Interfaces;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Threading.RateLimiting;
using System.Linq.Dynamic.Core.Tokenizer;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

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
                    builder.WithExposedHeaders("X-Pagination");
                });
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
            });

        public static void ConfigureLoggerManager(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        public static void ConfigureSQLContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("default"));
            });
        }
        //Define new Media Type - HATEOAS
        public static void AddCustomMediaTypes(this IServiceCollection services) 
        { 
            services.Configure<MvcOptions>(config => 
                { 
                    var systemTextJsonOutputFormatter = config.OutputFormatters.OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();
                    if (systemTextJsonOutputFormatter != null)
                    {
                        systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.hateoas+json");
                        systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.apiroot+json");
                    }
                    var xmlOutputFormatter = config.OutputFormatters.OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();
                    if (xmlOutputFormatter != null)
                    { 
                        xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.hateoas+xml");
                        xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.apiroot+xml");
                    } 
                }); 
        }
        //Adding Versioning Ability to API (needs Microsoft.AspNetCore.Mvc.Versioning nuget package)
        public static void ConfigureVersioning(this IServiceCollection services) 
        { 
            services.AddApiVersioning(opt => 
                { 
                    opt.ReportApiVersions = true;
                    opt.AssumeDefaultVersionWhenUnspecified = true; 
                    opt.DefaultApiVersion = new ApiVersion(1, 0);
                    //opt.ApiVersionReader = new HeaderApiVersionReader("api-version"); we can add versioning through header
                });
        }

        //Adding Cache-Store
        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }
        //Add Output Cache (.NET 7)
        public static void ConfigureOutputCaching(this IServiceCollection services)
        {
            services.AddOutputCache(opt =>
            {
                //opt.AddBasePolicy(policy => policy.Expire(TimeSpan.FromSeconds(10))); //to add policy for Output Cache
                //opt.AddPolicy("20SecondsOutputCache", policy => policy.Expire(TimeSpan.FromSeconds(20)));  //to add policy profile for Output Cache
            });
        }

        //Rate Limiting & Throttling
        public static void ConfigureRateLimitingOptions(this IServiceCollection services) 
        { 
            services.AddRateLimiter(opt => 
                {
                    //Set Global Limiter
                    opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>
                        (context => RateLimitPartition.
                            GetFixedWindowLimiter("GlobalLimiter", partition => new FixedWindowRateLimiterOptions
                            {
                                AutoReplenishment = true,
                                PermitLimit = 5,
                                QueueLimit = 2,
                                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                Window = TimeSpan.FromMinutes(1)
                            })
                         );
                    //To set response code if the limit exceeded
                    //opt.RejectionStatusCode = 429;

                    //Add Specific Limiter Policy
                    opt.AddPolicy("SpecificPolicy", context =>
                        RateLimitPartition.GetFixedWindowLimiter("SpecificLimiter",
                        partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 3,
                            Window = TimeSpan.FromSeconds(10)
                        }));

                    opt.OnRejected = async (context, token) =>
                    {
                        context.HttpContext.Response.StatusCode = 429;

                        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                            await context.HttpContext.Response
                                .WriteAsync($"Too many requests. Please try again after {retryAfter.TotalSeconds} second(s).", token);
                        else
                            await context.HttpContext.Response
                                .WriteAsync("Too many requests. Please try again later.", token);
                    };

                });
        }

        //Authentication With ASP.NET Core Identity
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(option => 
                { 
                    option.Password.RequireDigit = true;
                    option.Password.RequireLowercase = false;
                    option.Password.RequireUppercase = false; 
                    option.Password.RequireNonAlphanumeric = false; 
                    option.Password.RequiredLength = 6; 
                    option.User.RequireUniqueEmail = true;
                }).AddEntityFrameworkStores<RepositoryContext>().AddDefaultTokenProviders(); }

    }
}
