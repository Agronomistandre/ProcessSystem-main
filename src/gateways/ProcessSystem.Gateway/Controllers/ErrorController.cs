using System;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProcessSystem.Contracts;

namespace ProcessSystem.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController>? logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("/error")]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();


            _logger?.LogCritical($"ErrorController fatal error: {context.Error.Message}" +
                                 $" \n {context.Error} " +
                                 $" \n {context.Error.StackTrace} " +
                                 $" \n {context.Error.InnerException}");

            return new ObjectResult(new  BaseResponse<object>()
            {
                ErrorDescription = context.Error.Message,
                ErrorCode = context.Error.StackTrace,
            })
            {
                StatusCode = (int)HttpStatusCode.BadGateway
            };


        }
    }
}