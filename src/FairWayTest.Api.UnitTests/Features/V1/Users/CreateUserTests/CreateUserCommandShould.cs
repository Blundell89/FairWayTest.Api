using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
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
    public class CreateUserCommandShould
    {
        private IMongoCollection<User> _collection;
        private CommandResult _result;
        private User _user;

        [OneTimeSetUp]
        public async Task WhenCreateUserCommandDispatched()
        {
            var request = new Fixture().Create<CreateUser.Command>();

            var database = Substitute.For<IMongoDatabase>();
            _collection = Substitute.For<IMongoCollection<User>>();
            database.GetCollection<User>("users").Returns(_collection);

            var validator = Substitute.For<IValidator<CreateUser.Command>>();
            validator.ValidateAsync(request, CancellationToken.None).Returns(new ValidationResult());

            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Is<FindBankDetailsByAccountNumber.Query>(x => x.AccountNumber == request.BankDetails.AccountNumber), CancellationToken.None).Returns(Maybe<BankDetails>.None());
            
            _user = new User();
            var mapper = Substitute.For<IMapper>();
            mapper.Map<User>(request).Returns(_user);

            var handler = new CreateUser(database, validator, mediator, mapper);
            
            _result = await handler.Handle(request, CancellationToken.None);
        }

        [Test]
        public async Task InsertANewUser() => await _collection.Received(1).InsertOneAsync(_user, cancellationToken: CancellationToken.None);

        [Test]
        public void ReturnSuccess() => _result.IsSuccess.Should().BeTrue();
    }
}
