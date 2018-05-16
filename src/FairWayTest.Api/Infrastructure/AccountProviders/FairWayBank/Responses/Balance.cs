using System;

namespace FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank.Responses
{
    public class Balance
    {
        public decimal Amount { get; set; }

        public string Type { get; set; }

        public Overdraft Overdraft { get; set; }

        public DateTime DateTime { get; set; }
    }
}