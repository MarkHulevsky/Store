using Microsoft.AspNetCore.Http;
using Store.BuisnessLogic.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace Store.Presentation.Middlewares
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggerMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync(ex.ToString());
            }
        }
    }
}
