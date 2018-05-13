using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace FairWayTest.Api.Features.V1.Users
{
    public class CreateUser : IRequestHandler<CreateUser.Command, CommandResult>
    {
        private readonly IValidator<Command> _validator;

        public class Command : IRequest<CommandResult>
        {
            public Guid Id { get; set; }

            public BankDetails BankDetails { get; set; }

            public string FirstName { get; set; }

            public string Surname { get; set; }
        }

        private readonly IMongoCollection<Command> _collection;

        public CreateUser(IMongoDatabase database, IValidator<Command> validator)
        {
            _validator = validator;
            _collection = database.GetCollection<Command>("users");
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);

            if (!validationResult.IsValid)
                return CommandResult.Fail(validationResult.Errors.First().ErrorMessage);

            await _collection.InsertOneAsync(request, cancellationToken: cancellationToken).ConfigureAwait(false);

            return CommandResult.Success;
        }
    }
}