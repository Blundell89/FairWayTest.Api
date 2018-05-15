using System.Threading;
using System.Threading.Tasks;
using FairWayTest.Api.Infrastructure;
using MediatR;
using MongoDB.Driver;

namespace FairWayTest.Api.Features.V1.Users
{
    public class FindBankDetailsByAccountNumber : IRequestHandler<FindBankDetailsByAccountNumber.Query, Maybe<BankDetails>>
    {
        private readonly IMongoCollection<User> _collection;

        public FindBankDetailsByAccountNumber(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<User>(MongoConstants.UsersCollectionName);
        }

        public async Task<Maybe<BankDetails>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userMaybe = await _collection.Find(x => x.BankDetails.AccountNumber == request.AccountNumber).SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (userMaybe == null)
                return Maybe<BankDetails>.None();

            return Maybe<BankDetails>.Some(userMaybe.BankDetails);
        }

        public class Query : IRequest<Maybe<BankDetails>>
        {
            public Query(string accountNumber)
            {
                AccountNumber = accountNumber;
            }

            public string AccountNumber { get; }
        }
    }
}