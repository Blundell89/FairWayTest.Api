using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank
{
    public class FairWayBankClient : HttpClient
    {
        public FairWayBankClient(IOptions<FairWayBankConfiguration> options)
        {
            BaseAddress = new Uri(options.Value.BaseUrl, UriKind.Absolute);

            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}