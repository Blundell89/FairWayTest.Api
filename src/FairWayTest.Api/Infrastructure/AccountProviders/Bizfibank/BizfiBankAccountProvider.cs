using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FairWayTest.Api.Features.V1.Accounts;

namespace FairWayTest.Api.Infrastructure.AccountProviders.Bizfibank
{
    public class BizfiBankAccountProvider : IAccountProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public BizfiBankAccountProvider(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<Maybe<Account>> TryGetAccount(string accountNumber, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"accounts/{accountNumber}", cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return Maybe<Account>.Some(_mapper.Map<Account>(await response.Content.ReadAsAsync<Responses.Account>(cancellationToken).ConfigureAwait(false)));
            }

            return Maybe<Account>.None();
        }

        public string Name => "BizfiBank";
    }
}