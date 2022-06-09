using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProcessSystem.Contracts;
using ProcessSystem.DB;
using ProcessSystem.Token;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ProcessSystem.UnitTests
{
    [TestClass]
    public class ShurundinRegisterTest
    {
        private readonly Mock<IRegisterRepository> _registerRepository = new Mock<IRegisterRepository>();
        private readonly Register _register = new Register(
            RegisterRepositoryMock.DefaultToken,
            RegisterRepositoryMock.DefaultUrl,
            RegisterRepositoryMock.DefaultName
         );


        public ShurundinRegisterTest()
        {
            //_registerRepository.Setup(r => r.FindByNameAndUrlAsync(_register));
        }

        /// <summary>
        /// Выполняет проверку для GenerateToken.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void GenerateTokenResultNotNullTest()
        {
            var token = new TokenImpl();
            var result = token.GenerateToken();
            Assert.IsNotNull(result);
        }


        /// <summary>
        /// Выполняет проверку для FindByNameAndUrlAsync.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ShowFindByNameAndUrlAsyncResult()
        {
            var registerRepository = new RegisterRepository(new ProcessContext());
            var result = registerRepository.FindByNameAndUrlAsync(new Register("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyIiwianRpIjoiYWFjZDhhNGQtNDQyMS00NWU5LTlhN2QtNzA5ZmVkMDc5MmIzIiwiZ" +
                 "XhwIjoxNjU0Nzk0MDk0LCJpc3MiOiJUZXN0IiwiYXVkIjoiVGVzdCJ9.e5G3iyDhqEUS8bzz4N0NGMTfCReoOravgFOChJV6FDA", "BadURL", "test"));
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Выполняет проверку для FindByTokenAsync.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ShowFindByTokenAsyncResult()
        {
            var registerRepository = new RegisterRepository(new ProcessContext());
            var result = registerRepository.FindByTokenAsync("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyIiwianRpIjoiYWFjZDhhNGQtNDQyMS00NWU5LTlhN2QtNzA5ZmVkMDc5MmIzIiwiZ" +
                 "XhwIjoxNjU0Nzk0MDk0LCJpc3MiOiJUZXN0IiwiYXVkIjoiVGVzdCJ9.e5G3iyDhqEUS8bzz4N0NGMTfCReoOravgFOChJV6FDA");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Что-то не то.
        /// </summary>
        [TestMethod]
        public void ShowRegisterRequestResult()
        {
            var registerRequest = new RegisterRequest();
            registerRequest.Name = "Test";
            var test = new ValidationContext(registerRequest);
            var result = registerRequest.Validate(test);
            
            Assert.AreEqual(result, "Url для ответа пустой");
        }

    }
}
