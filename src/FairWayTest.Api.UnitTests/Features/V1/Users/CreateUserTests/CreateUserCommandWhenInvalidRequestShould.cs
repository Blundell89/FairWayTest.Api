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
    public class CreateUserCommandWhenInvalidRequestShould
    {
        private CommandResult _result;
        private CreateUser.Command _request;

        [OneTimeSetUp]
        public async Task WhenInvalidCreateUserCommandDispatched()
        {
            _request = new Fixture().Create<CreateUser.Command>();

            var database = Substitute.For<IMongoDatabase>();
            var collection = Substitute.For<IMongoCollection<CreateUser.Command>>();
            database.GetCollection<CreateUser.Command>("users").Returns(collection);

            var validator = Substitute.For<IValidator<CreateUser.Command>>();
            validator.ValidateAsync(_request, CancellationToken.None).Returns(new ValidationResult(new []{new ValidationFailure("Prop1", "Error!"), }));

            var handler = new CreateUser(database, validator, null, null);

            _result = await handler.Handle(_request, CancellationToken.None);
        }

        [Test]
        public void ReturnFailure() => _result.IsSuccess.Should().BeFalse();

        [Test]
        public void ReturnAnErrorMessageContainingThePropertNameAndError() => _result.FailureReason.Should().Be("Error!");
    }
}