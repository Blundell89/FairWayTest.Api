using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FairWayTest.Api.Configuration;
using FairWayTest.Api.Features.V1.Accounts;
using FairWayTest.Api.Infrastructure.AccountProviders.Bizfibank;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace FairWayTest.Api.IntegrationTests.Infrastructure.BizfiBank
{
    public class BizfiBankAccountRequestShould
    {
        private Maybe<Account> _result;

        [OneTimeSetUp]
        public async Task WhenGettingAccountFromBizfiBank()
        {
            var provider = new BizfiBankAccountProvider(new BizfiBankClient(new OptionsWrapper<BizfiBankConfiguration>(new BizfiBankConfiguration
            {
                BaseUrl = "https://bizfibank-bizfitech.azurewebsites.net/api/v1/"
            })), new MapperConfiguration(x => x.AddProfile<BizfiBankAccountToAccountProfile>()).CreateMapper());

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
