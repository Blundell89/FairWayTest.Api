using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace FairWayTest.Api.FunctionalTests
{
    public class SuccessfullyGetAllValuesTests
    {
        [Test]
        public async Task GivenInit()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(Configuration.BaseUrl, UriKind.Absolute);

            var response = await client.GetAsync("api/values");
            var result = await response.Content.ReadAsAsync<string[]>();

            result.Length.Should().Be(2);
        }
    }
}
