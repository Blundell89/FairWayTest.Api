using System;
using System.Net.Http;
using System.Net.Http.Headers;
using FairWayTest.Api.Configuration;
using Microsoft.Extensions.Options;

namespace FairWayTest.Api.Infrastructure.AccountProviders.Bizfibank
{
    public class BizfiBankClient : HttpClient
    {
        public BizfiBankClient(IOptions<BizfiBankConfiguration> options)
        {
            BaseAddress = new Uri(options.Value.BaseUrl, UriKind.Absolute);

            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}