using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FairWayTest.Api.Infrastructure;
using MediatR;
using MongoDB.Driver;

namespace FairWayTest.Api.Features.V1.Users
{
    public class GetUsers : IRequestHandler<GetUsers.Query, ICollection<User>>
    {
        private readonly IMongoCollection<User> _collection;

        public GetUsers(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<User>(MongoConstants.UsersCollectionName);
        }

        public async Task<ICollection<User>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _collection.Find(x => true).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public class Query : IRequest<ICollection<User>>
        {}
    }
}