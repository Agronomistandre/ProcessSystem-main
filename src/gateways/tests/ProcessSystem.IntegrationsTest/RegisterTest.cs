using Newtonsoft.Json;
using ProcessSystem.IntegrationsTest;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using NUnit.Framework;

namespace ProcessSystem.UnitTests
{
    [TestFixture]
    public class RegisterTest
    {
        private TestServerWrap _server;

        [OneTimeSetUp]
        public void SetUp()
        {
            _server = new TestServerWrap(typeof(Startup), "appsettings");
        }

        [Test]
        public async Task RegisterAsync()
        {
            var json = JsonConvert.SerializeObject(
                new
                {
                    Channel = "RegisterSuccessTestChannel",
                    Url = "localhost:8532",
                    ProcessTypesList = new List<string>
                    {
                       "first",
                       "second"
                    },
                    Name = "Test1:)",
                },
                Formatting.Indented
            );


            var content = new StringContent(json);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var resserver = await _server.Client.PostAsync("api/Register/RegisterUrl", content);
            var data = new { Data = ""};
            var dod = await resserver.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeAnonymousType(dod, data);
            Assert.IsNotNull(result.Data);

        }
        

        [Test]
        public async Task UnregisterAsync()
        {
            var json = JsonConvert.SerializeObject(
               new
               {
                   Channel = "RegisterSuccessTestChannel",
                   Url = "Test1_tests",
                   ProcessTypesList = new List<string>
                   {
                       "first",
                       "second"
                   },
                   Name = "Test1:)",
               },
               Formatting.Indented
           );
            const string token = "iOptionsTestToken";
            _server.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(json);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var resserver = await _server.Client.PostAsync("api/Register/UnRegisterUrl", content);
            var atribute = new {Data = new { id = "", token = "", url = "", name = "", process_types = "" }};
            var dod = await resserver.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeAnonymousType(dod, atribute);
            Assert.IsNotNull(result.Data.id);
        }

        
    }
}
