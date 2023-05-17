using Contracts;
using Interfaces;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Service.DataShaping;
using Shared.Dtos.Costs;
using Shared.Dtos.Movies;
using StandardWebApiTemplate.Extensions;
using StandardWebApiTemplate.Presentation.ActionFilters;
using StandardWebApiTemplate.Utility;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
// Add services to the container.
builder.Services.ConfigureCors();
//Add Response Cache
//builder.Services.ConfigureResponseCaching();
//Add Output Cache (.NET 7)
//builder.Services.ConfigureOutputCaching();
//Add Rate Limiting
builder.Services.ConfigureRateLimitingOptions();

//Add Authentication Services
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
//Add JWT
builder.Services.ConfigureJWT(builder.Configuration);
//Adding IOption Configuration setting
builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerManager();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSQLContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; //This setting suppress the default model state validation that is implemented due to the existence of the [ApiController] attribute in all API controllers
});

builder.Services.AddScoped<ValidationFilterAttribute>(); //Register Custom ValidationFilterAttribute (For checking the Dto and ModelState)
builder.Services.AddScoped<ValidateMediaTypeAttribute>(); //Register Custom ValidateMediaTypeAttribute (HATEOAS)
builder.Services.AddScoped<ICostLinks, CostLinks>(); //Register ICostLinks (HATEOAS)

//DataShaper Classes
builder.Services.AddScoped<IDataShaper<CostDto>, DataShaper<CostDto>>(); 
builder.Services.AddScoped<IDataShaperNotHEATOAS<MovieDto>, DataShaperNotHEATOAS<MovieDto>>();

builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true; //To accept other output we need to add this settings
        config.ReturnHttpNotAcceptable = true; //To restirict the output to acceptable trpe
        config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 }); //Add chaching profile
    })
    .AddXmlDataContractSerializerFormatters() //To accept xml output
    .AddApplicationPart(typeof(StandardWebApiTemplate.Presentation.AssemblyRefrence).Assembly); //to add controller from another project
//Adding the custom media type extension for HATEOAS
builder.Services.AddCustomMediaTypes();
//Versioning
builder.Services.ConfigureVersioning();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if(app.Environment.IsProduction())
    app.UseHsts();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

//Add Rate Limiter
app.UseRateLimiter();

app.UseCors("CorsPolicy");

//Add Cache Middleware
//app.UseResponseCaching();

//Add Output Cache
//app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
