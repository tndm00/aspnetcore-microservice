using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Middlewares
{
    public class ErrorWrappingMiddleware
    {
        private ILogger _logger;
        private readonly RequestDelegate _next;

        public ErrorWrappingMiddleware(RequestDelegate next, ILogger logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var errorMsg = string.Empty;
            try
            {
                await _next.Invoke(context);
            }
            catch(ValidationException ex)
            {
                _logger.Error(ex, "An error occurred white run with Ocelot");
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
