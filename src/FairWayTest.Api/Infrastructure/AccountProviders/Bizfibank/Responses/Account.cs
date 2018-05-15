using Newtonsoft.Json;

namespace FairWayTest.Api.Infrastructure.AccountProviders.Bizfibank.Responses
{
    public class Account
    {
        [JsonProperty("account_name")]
        public string Name { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("sort_code")]
        public string SortCode { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("available_balance")]
        public decimal AvailableBalance { get; set; }

        [JsonProperty("overdraft")]
        public decimal? Overdraft { get; set; }
    }
}