using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FairWayTest.Api.Features.V1.Accounts;
using FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace FairWayTest.Api.IntegrationTests.Infrastructure.FairWayBank
{
    public class FairWayBankAccountRequestShould
    {
        private Maybe<Account> _result;

        [OneTimeSetUp]
        public async Task WhenGettingAccountFromFairWayBank()
        {
            var provider = new FairWayBankAccountProvider(new FairWayBankClient(new OptionsWrapper<FairWayBankConfiguration>(new FairWayBankConfiguration
            {
                BaseUrl = "https://fairwaybank-bizfitech.azurewebsites.net/api/v1/"
            })), new MapperConfiguration(x => x.AddProfile<FairWayBankAccountToAccountProfile>()).CreateMapper());

            _result = await provider.TryGetAccount("12345678", CancellationToken.None);
        }

        [Test]
        public void ReturnAResult() => _result.HasValue.Should().BeTrue();

        [Test]
        public void PopulateTheAccountWithValues()
        {
            var result = _result.Value;

            result.AccountNumber.Should().NotBeEmpty();
            result.AvailableBalance.Should().NotBe(0);
            result.Balance.Should().NotBe(0);
            result.Name.Should().NotBeEmpty();
            result.SortCode.Should().NotBeEmpty();
        }
    }
}
