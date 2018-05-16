using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FairWayTest.Api.Features.V1.Accounts;

namespace FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank
{
    public class FairWayBankAccountProvider : IAccountProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public FairWayBankAccountProvider(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<Maybe<Account>> TryGetAccount(string accountNumber, CancellationToken cancellationToken)
        {
            var accountResponse = await _httpClient.GetAsync($"accounts/{accountNumber}", cancellationToken).ConfigureAwait(false);

            if (!accountResponse.IsSuccessStatusCode)
                return Maybe<Account>.None();

            var account = _mapper.Map<Account>(await accountResponse.Content.ReadAsAsync<Responses.Account>(cancellationToken).ConfigureAwait(false));

            var balanceResponse = await _httpClient.GetAsync($"accounts/{accountNumber}/balance", cancellationToken).ConfigureAwait(false);

            account = _mapper.Map(await balanceResponse.Content.ReadAsAsync<Responses.Balance>(cancellationToken).ConfigureAwait(false), account);

            return Maybe<Account>.Some(account);
        }

        public string Name => "FairWayBank";
    }
}