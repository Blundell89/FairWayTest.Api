using System.Threading;
using System.Threading.Tasks;
using FairWayTest.Api.Features.V1.Accounts;

namespace FairWayTest.Api.Infrastructure.AccountProviders
{
    public interface IAccountProvider
    {
        Task<Maybe<Account>> TryGetAccount(string accountNumber, CancellationToken cancellationToken);

        string Name { get; }
    }
}