using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NLog;
using ServerForReact.CustomExceptionMiddleware;
using ServerForReact.Models.ErrorDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ServerForReact.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public static void ConfigureExceptionhandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
                {
                    appError.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null)
                        {
                            logger.Error($"Smth went wrong: {contextFeature.Error}");

                            await context.Response.WriteAsync(new ErrorDetails
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = "Internal Server Error"
                            }.ToString());
                        }
                    });
                });
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
