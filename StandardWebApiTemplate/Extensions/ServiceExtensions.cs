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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.ConfigurationModels;
using Microsoft.OpenApi.Models;
using StandardWebApiTemplate.Presentation.Controllers;

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
        //Define new Media Type - HATEOAS & APIRoot
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
                    opt.ApiVersionReader = new HeaderApiVersionReader("api-version"); //we can add versioning through header
                    opt.Conventions.Controller<MoviesController>()
                                .HasApiVersion(new ApiVersion(1, 0));
                    opt.Conventions.Controller<MoviesV2Controller>()
                                    .HasApiVersion(new ApiVersion(2, 0));
                    //opt.Conventions.Controller<MoviesV2Controller>()
                    //    .HasDeprecatedApiVersion(new ApiVersion(2, 0));
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
                }).AddEntityFrameworkStores<RepositoryContext>().AddDefaultTokenProviders();
        }

        //Add JWT

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            //var jwtSettings = configuration.GetSection("JwtSettings"); For binding the setting we can use the belwo line instead of this line
            var jwtConfiguration = new JwtConfiguration();
            configuration.Bind(jwtConfiguration.Section, jwtConfiguration);

            //var secretKey = Environment.GetEnvironmentVariable("SECRET");
            var secretKey = configuration.GetSection("JwtSecters:JwtSecters").Value;
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = jwtSettings["validIssuer"], as we used configuration binding we can use the below lines instead of these two lines
                    //ValidAudience = jwtSettings["validAudience"], 
                    ValidIssuer = jwtConfiguration.ValidIssuer,
                    ValidAudience = jwtConfiguration.ValidAudience,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        //Adding IOption for Configurations
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));
        }

        //Configure Swagger
        public static void ConfigureSwagger(this IServiceCollection services) 
        { 
            services.AddSwaggerGen(s => 
            { 
                s.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Standard Web Api Template",
                    Version = "v1",
                    Description = "Movie API by Amin",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact 
                        { 
                            Name = "Amin Seifoori",
                            Email = "amin.seifoori@gmail.com"
                        },
                    License = new OpenApiLicense 
                        { 
                            Name = "Movie API", 
                            Url = new Uri("https://example.com/license"),
                        }
                });
                s.SwaggerDoc("v2", new OpenApiInfo { Title = "Standard Web Api Template", Version = "v2" });
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
                { 
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer" 
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement() 
                { 
                    { 
                        new OpenApiSecurityScheme 
                        { 
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme, 
                                Id = "Bearer"
                            }, 
                            Name = "Bearer", 
                        }, 
                        new List<string>() 
                    } 
                });
            }); 
        }
    }
}
