using FluentValidation;

namespace FairWayTest.Api.Features.V1.Users.Requests
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator(IValidator<BankDetails> bankDetailsValidator)
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.BankDetails).NotNull().SetValidator(bankDetailsValidator);
        }
    }
}