using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FairWayTest.Api.Features.V1.Users;
using FairWayTest.Api.Features.V1.Users.Validators;
using FluentAssertions;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users.Validators
{
    public class BankDetailsValidatorShould
    {
        private BankDetailsValidator _validator;

        [SetUp]
        public void WhenValidatingBankDetails()
        {
            _validator = new BankDetailsValidator();
        }

        [Test]
        public void ReturnValidWhenAllPropsCorrect()
        {
            var bankDetails = new Fixture().Build<BankDetails>()
                .With(x => x.AccountNumber, "12345678")
                .Create();

            _validator.Validate(bankDetails).IsValid.Should().BeTrue();
        }

        [Test]
        public void ReturnInvalidWhenAccountNumberUnderEightCharacters()
        {
            var bankDetails = new Fixture().Build<BankDetails>()
                .With(x => x.AccountNumber, "1234567")
                .Create();

            _validator.Validate(bankDetails).IsValid.Should().BeFalse();
            _validator.Validate(bankDetails).Errors.Single().ErrorMessage.Should().Be("Account number must be 8 digits and not start with 0.");
        }

        [Test]
        public void ReturnInvalidWhenAccountNumberOverEightCharacters()
        {
            var bankDetails = new Fixture().Build<BankDetails>()
                .With(x => x.AccountNumber, "123456789")
                .Create();

            _validator.Validate(bankDetails).IsValid.Should().BeFalse();
        }

        [Test]
        public void ReturnInvalidWhenAccountNumberStartsWithZero()
        {
            var bankDetails = new Fixture().Build<BankDetails>()
                .With(x => x.AccountNumber, "01234567")
                .Create();

            _validator.Validate(bankDetails).IsValid.Should().BeFalse();
        }

        [Test]
        public void ReturnInvalidWhenNoBankName()
        {
            var bankDetails = new Fixture().Build<BankDetails>()
                .With(x => x.AccountNumber, "12345678")
                .Without(x => x.Name)
                .Create();

            _validator.Validate(bankDetails).IsValid.Should().BeFalse();
        }

        [Test]
        public void ReturnInvalidWhenNoSortCode()
        {
            var bankDetails = new Fixture().Build<BankDetails>()
                .With(x => x.AccountNumber, "12345678")
                .Without(x => x.SortCode)
                .Create();

            _validator.Validate(bankDetails).IsValid.Should().BeFalse();
        }
    }
}