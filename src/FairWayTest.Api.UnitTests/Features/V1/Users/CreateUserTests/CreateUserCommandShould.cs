using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FairWayTest.Api.Features.V1.Users;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users.CreateUserTests
{
    public class CreateUserCommandShould
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

            var handler = new CreateUser(database, validator);
            
            _result = await handler.Handle(_request, CancellationToken.None);
        }

        [Test]
        public async Task InsertANewUser() => await _collection.Received(1).InsertOneAsync(_request, cancellationToken: CancellationToken.None);

        [Test]
        public void ReturnSuccess() => _result.IsSuccess.Should().BeTrue();
    }
}
