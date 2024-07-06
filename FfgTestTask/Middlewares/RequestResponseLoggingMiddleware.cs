using Azure;
using FfgTestTask.Services.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;

namespace FfgTestTask.Middlewares
{
    /// <summary>
    /// Логгирование запросов и ответов к API в конвейере обработки запроса
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAppLogger logger)
        {
            Dictionary<string, object> request = null;
            Dictionary<string, object> response = null;
            Dictionary<string, object> error = null;

            try
            {
                request = await LogRequestAsync(context);

                await _next(context);

                response = await LogResponseAsync(context);
            }
            catch (Exception ex)
            {
                error = LogError(ex);

                throw;
            }
            finally
            {
                await logger.LogAsync(
                    $"Method: {context.Request.Method}. Path: {context.Request.Path}",
                    new Dictionary<string, object>
                    {
                        { "Request", request},
                        { "Response", response },
                        { "Error", error }
                    });
            }
        }

        /// <summary>
        /// Логгирование запроса 
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <returns></returns>
        private async Task<Dictionary<string, object>> LogRequestAsync(HttpContext context)
        {
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync(); 

            return new Dictionary<string, object>
            {
                { "DateTime", DateTime.Now },
                { "Method", context.Request.Method },
                { "QueryString", context.Request.QueryString},
                { "RequestBody", requestBody },
            };
        }

        /// <summary>
        /// Логгирование ошибок
        /// </summary>
        /// <param name="exception">Объект ошибки</param>
        /// <returns></returns>
        private Dictionary<string, object> LogError(Exception exception)
        {
            return new Dictionary<string, object>
            {
                { "DateTime", DateTime.Now },
                { "ExceptionType", exception.GetType().Name },
                { "ExceptionMsg" , exception.Message }
            };
        }

        /// <summary>
        /// Логгирование ответа запроса
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <returns></returns>
        private async Task<Dictionary<string, object>> LogResponseAsync(HttpContext context)
        {
            var responseBody = new StreamReader(context.Response.Body).ReadToEndAsync();

            return new Dictionary<string, object>
            {
                { "DateTime", DateTime.Now },
                { "ResponseBody", responseBody },
            };
        }
    }
}
