using Microsoft.AspNetCore.Http;
using Store.BuisnessLogic.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace Store.Presentation.Middlewares
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public LoggerMiddleware(RequestDelegate next, ILogger logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await logger.LogAsync(ex.ToString());
            }
        }
    }
}
