using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FairWayTest.Api.Features.V1.Accounts;
using FairWayTest.Api.Infrastructure.AccountProviders.Bizfibank;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Infrastructure.AccountProviderTests.BizfiBank.BizfiBankAccountProviderTests
{
    public class BizfiBankAccountProviderShould
    {
        private Account _expected;
        private Maybe<Account> _result;
        private BizfiBankAccountProvider _bizfiBankAccountProvider;
        private const string _response = "{\"account_name\": \"Current Account\",\"account_number\": \"12345678\",\"sort_code\": \"405431\",\"balance\": -8141.75,\"available_balance\": -7141.75,\"overdraft\": 1000}";

        [OneTimeSetUp]
        public async Task WhenGettingAccount()
        {
            var accountNumber = "12345678";

            _expected = new Account();

            var stubHttpMessageHandler = new StubHttpMessageHandlerBuilder().WithResponse(new Uri($"https://test.local/accounts/{accountNumber}"), _response)
                                                                            .Build();
            var httpClient = new HttpClient(stubHttpMessageHandler) {BaseAddress = new Uri("https://test.local")};
            var mapper = Substitute.For<IMapper>();
            mapper.Map<Account>(Arg.Any<FairWayTest.Api.Infrastructure.AccountProviders.Bizfibank.Responses.Account>()).Returns(_expected);

            _bizfiBankAccountProvider = new BizfiBankAccountProvider(httpClient, mapper);
            _result = await _bizfiBankAccountProvider.TryGetAccount(accountNumber, new CancellationToken());
        }

        [Test]
        public void ReturnAValue() => _result.HasValue.Should().BeTrue();

        [Test]
        public void ReturnTheAccount() => _result.Value.Should().BeSameAs(_expected);

        [Test]
        public void HaveTheProviderNameBizfiBank() => _bizfiBankAccountProvider.Name.Should().Be("BizfiBank");
    }
}
