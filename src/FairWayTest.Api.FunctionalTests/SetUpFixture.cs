using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace FairWayTest.Api.FunctionalTests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        private static readonly CancellationToken Token = new CancellationToken();
        private IWebHost _webHost;

        [OneTimeSetUp]
        public async Task Start()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection()
                .Build();

            _webHost = WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .UseUrls(Configuration.BaseUrl)
                .UseStartup<Startup>()
                .Build();

            await _webHost.StartAsync(Token);
        }

        [OneTimeTearDown]
        public async Task Stop()
        {
            await _webHost.StopAsync(Token);
        }
    }
}