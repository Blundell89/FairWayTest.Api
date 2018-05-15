namespace FairWayTest.Api.Features.V1.Accounts
{
    public class Account
    {
        public string Name { get; set; }

        public string AccountNumber { get; set; }

        public string SortCode { get; set; }

        public decimal Balance { get; set; }

        public decimal AvailableBalance { get; set; }

        public decimal? Overdraft { get; set; }
    }
}