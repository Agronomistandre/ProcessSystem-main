using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProcessSystem.UI.Blazor.Server.ProcessSystemClient;
using ProcessSystem.UI.Blazor.Server.Services;

namespace ProcessSystem.UI.Blazor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {

        private readonly ILogger<RegisterController> _logger;
        private readonly IClient _processSystemClient;
        private ITokenCacheService _cacheService;
        public RegisterController(ILogger<RegisterController> logger, IClient processSystemClient, ITokenCacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
            _processSystemClient = processSystemClient;
        }

        [HttpPost("Register")]
        public async Task<string> Get([FromBody] RegisterRequest registerRequest)
        {
            //var result = await _processSystemClient.ApiRegisterRegisterurlAsync(registerRequest);
            var result = new BaseResponse_1OfString { Data = Guid.NewGuid().ToString() };

            _cacheService.SetAccessToken("token", result.Data);

            return await Task.FromResult(result.Data);
        }

        [HttpGet("CheckToken")]
        public async Task<string> CheckToken()//Просто проверяю кеш
        {
            return _cacheService.GetAccessToken("token");
        }
    }
}
