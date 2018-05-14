using System;

namespace FairWayTest.Api.Features.V1.Users
{
    public class User
    {
        public Guid Id { get; set; }

        public BankDetails BankDetails { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }
    }
}