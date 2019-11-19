using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests
{

    // dotnet add package Microsoft.AspNetCore.TestHost
    // dotnet add package Microsoft.AspNetCore.App
    public class Tests
    {
        private TestServer server;
        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Api.Startup>()
                .UseEnvironment("Development"));

            client = server.CreateClient();

        }

        [TearDown]
        public void Down()
        {
            server.Dispose();
            client.Dispose();
        }

        [Test]
        public async Task Index_Get_ReturnHelloWorld()
        {
            // Act
            var response = await client.GetAsync("api/tracking");

            string json = await response.Content.ReadAsStringAsync();

            // Assert

            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That(json, Does.Contain("Hello World"));
        }
    }
}