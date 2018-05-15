using System.Linq;
using AutoFixture;
using FairWayTest.Api.Features.V1.Users.Requests;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users.Requests
{
    public class CreateUserRequestValidatorShould
    {
        private CreateUserRequestValidator _validator;

        [SetUp]
        public void WhenValidatingBankDetails()
        {
            var bankDetailsValidator = Substitute.For<IValidator<BankDetails>>();
            bankDetailsValidator.Validate(Arg.Any<ValidationContext>()).Returns(new ValidationResult());

            _validator = new CreateUserRequestValidator(bankDetailsValidator);
        }

        [Test]
        public void ReturnValidWhenAllPropsCorrect()
        {
            var createUser = new Fixture().Build<CreateUserRequest>()
                .Create();

            _validator.Validate(createUser).IsValid.Should().BeTrue();
        }

        [Test]
        public void ReturnInvalidWhenFirstNameEmpty()
        {
            var createUser = new Fixture().Build<CreateUserRequest>()
                .Without(x => x.FirstName)
                .Create();

            _validator.Validate(createUser).IsValid.Should().BeFalse();
        }

        [Test]
        public void ReturnInvalidWhenSurnameEmpty()
        {
            var createUser = new Fixture().Build<CreateUserRequest>()
                .Without(x => x.Surname)
                .Create();

            _validator.Validate(createUser).IsValid.Should().BeFalse();
        }

        [Test]
        public void ReturnInvalidWhenBankDetailsEmpty()
        {
            var createUser = new Fixture().Build<CreateUserRequest>()
                .Without(x => x.BankDetails)
                .Create();

            _validator.Validate(createUser).IsValid.Should().BeFalse();
        }

        [Test]
        public void ReturnTheErrorAndPropertyNameInTheErrorMessage()
        {
            var createUser = new Fixture().Build<CreateUserRequest>()
                .Without(x => x.BankDetails)
                .Create();

            _validator.Validate(createUser).Errors.Single().ErrorMessage.Should().Be("'Bank Details' must not be empty.");
        }
    }
}