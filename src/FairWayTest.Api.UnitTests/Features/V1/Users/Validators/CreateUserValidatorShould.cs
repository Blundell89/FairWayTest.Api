using System;
using System.Linq;
using AutoFixture;
using FairWayTest.Api.Features.V1.Users;
using FairWayTest.Api.Features.V1.Users.Validators;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users.Validators
{
    public class CreateUserValidatorShould
    {
        private CreateUserValidator _validator;

        [SetUp]
        public void WhenValidatingBankDetails()
        {
            var createUser = Substitute.For<IValidator<BankDetails>>();

            _validator = new CreateUserValidator(createUser);
        }

        [Test]
        public void ReturnValidWhenAllPropsCorrect()
        {
            var createUser = new Fixture().Build<CreateUser.Command>()
                .Create();

            _validator.Validate(createUser).IsValid.Should().BeTrue();
        }

        [Test]
        public void ReturnInvalidWhenFirstNameEmpty()
        {
            var createUser = new Fixture().Build<CreateUser.Command>()
                .Without(x => x.FirstName)
                .Create();

            _validator.Validate(createUser).IsValid.Should().BeFalse();
        }

        [Test]
        public void ReturnInvalidWhenSurnameEmpty()
        {
            var createUser = new Fixture().Build<CreateUser.Command>()
                .Without(x => x.Surname)
                .Create();

            _validator.Validate(createUser).IsValid.Should().BeFalse();
        }

        [Test]
        public void ReturnInvalidWhenBankDetailsEmpty()
        {
            var createUser = new Fixture().Build<CreateUser.Command>()
                .Without(x => x.BankDetails)
                .Create();

            _validator.Validate(createUser).IsValid.Should().BeFalse();
        }

        [Test]
        public void ReturnInvalidWhenIdEmpty()
        {
            var createUser = new Fixture().Build<CreateUser.Command>()
                .Without(x => x.Id)
                .Create();

            _validator.Validate(createUser).IsValid.Should().BeFalse();
        }

        [Test]
        public void ReturnTheErrorAndPropertyNameInTheErrorMessage()
        {
            var createUser = new Fixture().Build<CreateUser.Command>()
                .Without(x => x.Id)
                .Create();

            _validator.Validate(createUser).Errors.Single().ErrorMessage.Should().Be("'Id' should not be empty.");
        }
    }
}