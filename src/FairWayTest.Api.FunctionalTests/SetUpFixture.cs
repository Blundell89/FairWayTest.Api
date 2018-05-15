using System.Collections.Generic;
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
            var config = new ConfigurationBuilder().AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("FairWayTest.Api:ConnectionString", Configuration.ConnectionString), 
                    new KeyValuePair<string, string>("BizfiBank:BaseUrl", Configuration.BizfiBankUrl), 
                })
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