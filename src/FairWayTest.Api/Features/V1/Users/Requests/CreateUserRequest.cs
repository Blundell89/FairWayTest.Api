namespace FairWayTest.Api.Features.V1.Users.Requests
{
    public class CreateUserRequest
    {
        public BankDetails BankDetails { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }
    }
}