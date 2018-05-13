using FluentValidation;

namespace FairWayTest.Api.Features.V1.Users.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUser.Command>
    {
        public CreateUserValidator(IValidator<BankDetails> bankDetailsValidator)
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.BankDetails).NotNull();
        }
    }
}