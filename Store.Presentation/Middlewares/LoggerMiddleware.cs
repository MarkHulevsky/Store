using Microsoft.AspNetCore.Http;
using Store.BuisnessLogicLayer.Common.Interfaces;
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
            catch(Exception ex)
            {
                logger.Log(ex.ToString());
            }
        }
    }
}
