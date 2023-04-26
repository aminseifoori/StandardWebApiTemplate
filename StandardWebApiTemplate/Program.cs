using Interfaces;
using Microsoft.AspNetCore.Mvc;
using NLog;
using StandardWebApiTemplate.Extensions;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
// Add services to the container.

builder.Services.AddControllers();
builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerManager();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSQLContext(builder.Configuration);
builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true; //To accespt other output we need to add this settings
        config.ReturnHttpNotAcceptable = true; //To restirict the output to acceptable trpe
    })
    .AddXmlDataContractSerializerFormatters() //To accept xml output
    .AddApplicationPart(typeof(StandardWebApiTemplate.Presentation.AssemblyRefrence).Assembly); //to add controller from another project
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; //This setting suppress the default model state validation that is implemented due to the existence of the [ApiController] attribute in all API controllers
});
builder.Services.AddAutoMapper(typeof(Program));



var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if(app.Environment.IsProduction())
    app.UseHsts();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
