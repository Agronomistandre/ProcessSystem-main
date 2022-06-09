using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using ProcessSystem.Contracts;
using ProcessSystem.Implementation;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ProcessSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiVersion("1.0")]
    public class StartProcessController : ControllerBase
    {
        private readonly IStartProcessService _startProcessService;
        private readonly ILogger<StartProcessController> _logger;
        public StartProcessController(IStartProcessService startProcessService, ILogger<StartProcessController> logger)
        {
            _startProcessService = startProcessService;
            _logger = logger;
        }

        [HttpPost("StartProcess")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> StartProcess()
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString();
                return await _startProcessService.ProcessStartAsync(token.Replace("Bearer ", ""));
            }
            catch (Exception ex) when
                       (ex is ArgumentNullException ||
                        ex is ArgumentOutOfRangeException)
            {

                _logger?.LogError(ex, $"StartProcess {ex.Message}");
                return new ObjectResult(
                    new BaseResponse<object>
                    {
                        ErrorDescription = ex.Message,
                        ErrorCode = "StartProcessError"
                    })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

        }
    }
}
