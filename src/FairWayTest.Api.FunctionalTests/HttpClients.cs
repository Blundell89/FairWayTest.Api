using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace FairWayTest.Api.FunctionalTests
{
    public static class HttpClients
    {
        static HttpClients()
        {
            FairWayApi = System.Net.Http.HttpClientFactory.Create();
            FairWayApi.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            FairWayApi.DefaultRequestHeaders.Add("Api-Version", "1.0");
            FairWayApi.BaseAddress = new Uri(Configuration.BaseUrl, UriKind.Absolute);
        }

        public static HttpClient FairWayApi { get; }
    }
}
