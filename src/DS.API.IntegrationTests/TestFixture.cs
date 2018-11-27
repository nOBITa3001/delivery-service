using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;

namespace DS.API.IntegrationTests
{
    public class TestFixture : IDisposable
    {
        private readonly TestServer _server;

        public HttpClient Client { get; }

        public TestFixture()
        {
            var builder = CreateWebHostBuilder();

            _server = new TestServer(builder);
            Client = _server.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }

        private IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    configBuilder.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../../../DS.API"));
                    configBuilder.AddJsonFile("appsettings.json");
                    configBuilder.AddJsonFile("appsettings.Testing.json");
                })
                .UseEnvironment("Testing");
        }
    }
}
