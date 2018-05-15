using System;
using System.Threading;
using System.Threading.Tasks;
using FairWayTest.Api.Features.V1.Accounts;

namespace FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank
{
    public class FairWayBankAccountProvider : IAccountProvider
    {
        public Task<Maybe<Account>> TryGetAccount(string accountNumber, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public string Name => "FairWayBank";
    }
}