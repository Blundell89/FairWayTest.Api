using FluentValidation;

namespace FairWayTest.Api.Features.V1.Users.Validators
{
    public class BankDetailsValidator : AbstractValidator<BankDetails>
    {
        public BankDetailsValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.AccountNumber).Length(8);
            RuleFor(x => x.SortCode).NotEmpty();
        }
    }
}