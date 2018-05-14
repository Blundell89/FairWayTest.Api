using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FairWayTest.Api.Features.V1.Users;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users.CreateUserTests
{
    public class CreateUserCommandWhenAccountNumberExistsShould
    {
        private IMongoCollection<CreateUser.Command> _collection;
        private CommandResult _result;
        private CreateUser.Command _request;

        [OneTimeSetUp]
        public async Task WhenCreateUserCommandDispatched()
        {
            _request = new Fixture().Create<CreateUser.Command>();

            var database = Substitute.For<IMongoDatabase>();
            _collection = Substitute.For<IMongoCollection<CreateUser.Command>>();
            database.GetCollection<CreateUser.Command>("users").Returns(_collection);

            var validator = Substitute.For<IValidator<CreateUser.Command>>();
            validator.ValidateAsync(_request, CancellationToken.None).Returns(new ValidationResult());

            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Is<FindBankDetailsByAccountNumber.Query>(x => x.AccountNumber == _request.BankDetails.AccountNumber), CancellationToken.None).Returns(Maybe<BankDetails>.Some(new BankDetails()));

            var handler = new CreateUser(database, validator, mediator, null);

            _result = await handler.Handle(_request, CancellationToken.None);
        }

        [Test]
        public async Task NotInsertANewUser() => await _collection.DidNotReceive().InsertOneAsync(_request, cancellationToken: CancellationToken.None);

        [Test]
        public void ReturnFailure() => _result.IsSuccess.Should().BeFalse();
    }
}