using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProcessSystem.Contracts;
using ProcessSystem.Implementation;
using ProcessSystem.Token;
using ProcessSystem.V1;

namespace ProcessSystem.UnitTests
{
    [TestClass]
    public class RegisterTest
    {
        private readonly Mock<ILogger<RegisterController>> _logger = new Mock<ILogger<RegisterController>>();
        private readonly Mock<IToken> _token = new Mock<IToken>();
        private readonly Register _register = new Register(
            RegisterRepositoryMock.DefaultToken,
            RegisterRepositoryMock.DefaultUrl,
            RegisterRepositoryMock.DefaultUrl
        );
        private readonly Mock<IOptions<TokenParam>> _options = new Mock<IOptions<TokenParam>>();


        public RegisterTest()
        {
            _token.Setup(t => t.GenerateToken()).Returns(RegisterRepositoryMock.DefaultToken);
            _options.Setup(o => o.Value).Returns(new TokenParam() { Token = RegisterRepositoryMock.DefaultToken });
        }

        [TestMethod]
        public async Task ShopWindowRegisterSuccessTest()
        {
            var repositoryMock = new RegisterRepositoryMock();
            repositoryMock.SetupAddSuccess();
            repositoryMock.SetupFindByChannelAndUrlFailure();
            repositoryMock.SetupUnitOfWorkSuccess();

            var controller = new RegisterController(_logger.Object, _token.Object, repositoryMock.GetMockObject(), _options.Object);

            var res = await controller.Register(
                new RegisterRequest()
                {
                    Url = RegisterRepositoryMock.DefaultUrl,
                    Name = RegisterRepositoryMock.DefaultName,
                    ProcessTypesList = RegisterRepositoryMock.DefaultProcessList
                });

            Assert.AreEqual(RegisterRepositoryMock.DefaultToken, ((BaseResponse<string>)((CreatedAtActionResult)res.Result).Value).Data);
        }

        [TestMethod]
        public async Task ShopWindowAlreadyRegisteredErrorTest()
        {
            var repositoryMock = new RegisterRepositoryMock();
            repositoryMock.SetupFindByChannelAndUrlSuccess();

            var controller = new RegisterController(_logger.Object, _token.Object, repositoryMock.GetMockObject(), _options.Object);
    

            var res = await controller.Register(
                new RegisterRequest()
                {
                    Url = RegisterRepositoryMock.DefaultUrl,
                    Name = RegisterRepositoryMock.DefaultName,
                    ProcessTypesList = RegisterRepositoryMock.DefaultProcessList
                });

            Assert.AreEqual($"Витрина с {RegisterRepositoryMock.DefaultUrl} и {RegisterRepositoryMock.DefaultName} уже зерегистрирована (Parameter 'registerRequest')", ((BaseResponse<object>)((ObjectResult)res.Result).Value).ErrorDescription);
        }

        [TestMethod]
        public async Task ShopWindowUnregisterSuccessTest()
        {
            var repositoryMock = new RegisterRepositoryMock();
            repositoryMock.SetupDeleteSuccess(_register);
            repositoryMock.SetupUnitOfWorkSuccess();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = RegisterRepositoryMock.DefaultToken;

            var controller = new RegisterController(_logger.Object, _token.Object, repositoryMock.GetMockObject(), _options.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                }
            };

            var res = await controller.UnRegister();

            Assert.AreEqual(_register.Token, ((BaseResponse<Register>)((CreatedAtActionResult)res.Result).Value).Data.Token);
        }

        [TestMethod]
        public async Task ShopWindowUnregisterErrorTest()
        {
            var repositoryMock = new RegisterRepositoryMock();
            repositoryMock.SetupDeleteFailure();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = RegisterRepositoryMock.DefaultToken;

            var controller = new RegisterController(_logger.Object, _token.Object, repositoryMock.GetMockObject(), _options.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            var res = await controller.UnRegister();

            Assert.AreEqual("UnRegisterError", ((BaseResponse<object>)((ObjectResult)res.Result).Value).ErrorCode);
        }
    }
}
