using NUnit.Framework;

namespace ProcessSystem.IntegrationsTest
{
    public class Tests
    {
        private TestServerWrap _server;
        [OneTimeSetUp]
        public void SetUp()
        {
            _server = new TestServerWrap(typeof(Startup), "appsettings");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _server?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}