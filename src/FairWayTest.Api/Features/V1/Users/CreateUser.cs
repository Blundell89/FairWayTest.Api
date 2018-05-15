using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace FairWayTest.Api.Features.V1.Users
{
    public class CreateUser : IRequestHandler<CreateUser.Command, CommandResult>
    {
        private readonly IMongoCollection<User> _collection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CreateUser(IMongoDatabase database, IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
            _collection = database.GetCollection<User>("users");
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var accountNumberExists = await AccountNumberExists(request, cancellationToken).ConfigureAwait(false);

            if (accountNumberExists)
                return CommandResult.Fail("Bank account registered to another user.");

            await InsertUser(request, cancellationToken).ConfigureAwait(false);

            return CommandResult.Success;
        }

        private async Task<bool> AccountNumberExists(Command request, CancellationToken cancellationToken)
        {
            var findBankDetailsQuery = new FindBankDetailsByAccountNumber.Query(request.BankDetails.AccountNumber);
            var bankDetailsMaybe = await _mediator.Send(findBankDetailsQuery, cancellationToken).ConfigureAwait(false);

            return bankDetailsMaybe.HasValue;
        }

        private async Task InsertUser(Command request, CancellationToken cancellationToken)
        {
            var mapped = _mapper.Map<User>(request);

            await _collection.InsertOneAsync(mapped, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public class Command : IRequest<CommandResult>
        {
            public Guid Id { get; set; }

            public BankDetails BankDetails { get; set; }

            public string FirstName { get; set; }

            public string Surname { get; set; }
        }

        public class BankDetails
        {
            public string Name { get; set; }

            public string AccountNumber { get; set; }

            public string SortCode { get; set; }
        }
    }
}