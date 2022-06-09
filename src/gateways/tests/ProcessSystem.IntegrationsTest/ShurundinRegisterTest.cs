using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSystem.IntegrationsTest
{
    [TestFixture]
    public class ShurundinRegisterTest
    {
        private TestServerWrap _server;

        [OneTimeSetUp]
        public void SetUp()
        {
            _server = new TestServerWrap(typeof(Startup), "appsettings");
        }

        /// <summary>
        /// Выполняет Register с пустым Name.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task BadlyRegisterAsync()
        {
            var json = JsonConvert.SerializeObject(
                new
                {
                    Channel = "BadlyRegisterTestChannel",
                    Url = "BadURL",
                    ProcessTypesList = new List<string>
                    {
                        "first",
                        "second"
                    },
                    Name = ""
                },
                Formatting.Indented
                );
            var content = new StringContent(json);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var resserver = await _server.Client.PostAsync("api/Register/RegisterUrl", content);
            var data = new { Data = "" };
            var dod = await resserver.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeAnonymousType(dod, data);
            Assert.IsNull(result.Data);
        }

        /// <summary>
        /// Выполняет Unregister c несуществующим токеном.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task BadlyUnregisterAsync()
        {
            var json = JsonConvert.SerializeObject(
               new
               {
                   Channel = "BadlyUnregisterTestChannel",
                   Url = "BadURL",
                   ProcessTypesList = new List<string>
                    {
                        "first",
                        "second"
                    },
                   Name = "test"
               },
               Formatting.Indented
           );
            //const string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyIiwianRpIjoiZWEyODM4OWQtMTIyMC00MWJiLWFiOWQtN2YzNDU4NGI4N2ZhIiwiZXhwIjoxNjU0Nzk1MDk3LCJpc3MiOiJUZXN0IiwiYXVkIjoiVGVzdCJ9.ENF0sWHF2GyVzRKzleDtaebC5IFI20EN-5T0aS-MJhY";
            const string token = "BadToken";
            _server.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(json);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var resserver = await _server.Client.PostAsync("api/Register/UnRegisterUrl",content);
            var atribute = new { Data = new { id = "", token = "", url = "", name = "", process_types = "" } };
            var dod = await resserver.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeAnonymousType(dod, atribute);
            Assert.IsNull(result.Data);
        }
    }
}
