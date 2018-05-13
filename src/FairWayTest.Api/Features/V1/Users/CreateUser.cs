using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;

namespace FairWayTest.Api.Features.V1.Users
{
    public class CreateUser : IRequestHandler<CreateUser.Command, CommandResult>
    {
        public class Command : IRequest<CommandResult>
        {
            public Guid Id { get; set; }

            public BankDetails BankDetails { get; set; }
        }

        private readonly IMongoCollection<Command> _collection;

        public CreateUser(IMongoDatabase database)
        {
            _collection = database.GetCollection<Command>("users");
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(request, cancellationToken: cancellationToken).ConfigureAwait(false);

            return CommandResult.Success;
        }
    }
}