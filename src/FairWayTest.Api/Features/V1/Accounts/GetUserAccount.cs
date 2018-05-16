using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FairWayTest.Api.Configuration;
using FairWayTest.Api.Features.V1.Users;
using FairWayTest.Api.Infrastructure;
using FairWayTest.Api.Infrastructure.AccountProviders;
using MediatR;
using MongoDB.Driver;

namespace FairWayTest.Api.Features.V1.Accounts
{
    public class GetUserAccount : IRequestHandler<GetUserAccount.Query, Maybe<Account>>
    {
        private readonly IEnumerable<IAccountProvider> _accountProviders;
        private readonly IMongoCollection<User> _collection;

        public GetUserAccount(IEnumerable<IAccountProvider> accountProviders, IMongoDatabase database)
        {
            _accountProviders = accountProviders;
            _collection = database.GetCollection<User>(MongoConstants.UsersCollectionName);
        }

        public async Task<Maybe<Account>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _collection.Find(x => x.Id == request.UserId).SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (user == null)
                return Maybe<Account>.None();

            var bankProvider = _accountProviders.SingleOrDefault(x => x.Name == user.BankDetails.Name);

            if (bankProvider == null)
                return Maybe<Account>.None();

            var result = await bankProvider.TryGetAccount(user.BankDetails.AccountNumber, cancellationToken).ConfigureAwait(false);

            return result.HasValue ? Maybe<Account>.Some(result.Value) : Maybe<Account>.None();
        }

        public class Query : IRequest<Maybe<Account>>
        {
            public Query(Guid userId)
            {
                UserId = userId;
            }

            public Guid UserId { get; }
        }
    }
}
