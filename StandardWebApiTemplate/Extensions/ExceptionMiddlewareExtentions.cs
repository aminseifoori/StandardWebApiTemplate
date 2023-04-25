using Domain.ErrorModel;
using Domain.Exceptions;
using Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace StandardWebApiTemplate.Extensions
{
    public static class ExceptionMiddlewareExtentions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if(contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
