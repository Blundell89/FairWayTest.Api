using FluentValidation;

namespace FairWayTest.Api.Features.V1.Users.Requests
{
    public class BankDetailsValidator : AbstractValidator<BankDetails>
    {
        public BankDetailsValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.AccountNumber).Matches(@"^[1-9]\d{7}$").WithMessage("Account number must be 8 digits and not start with 0.");
        }
    }
}