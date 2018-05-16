using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FairWayTest.Api.Features.V1.Accounts;
using FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Infrastructure.AccountProviderTests.FairWayBank.FairWayBankAccountProviderTests
{
    public class FairWayBankAccountProviderShould
    {
        private Account _expected;
        private Maybe<Account> _result;
        private FairWayBankAccountProvider _fairWayBankAccountProvider;
        private IMapper _mapper;
        private const string _accountResponse = "{\"name\": \"Current Account\",\"identifier\": {\"accountNumber\": \"12345678\", \"sortCode\": \"012345\"}}";
        private const string _balanceResponse = "{\"amount\": 8141.75,\"type\": \"Debit\",\"overdraft\": {\"amount\": 1000},\"dateTime\": \"2018-05-16T17:25:55.8616681+00:00\"}";

        [OneTimeSetUp]
        public async Task WhenGettingAccount()
        {
            var accountNumber = "12345678";

            _expected = new Account();

            var stubHttpMessageHandler = new StubHttpMessageHandlerBuilder().WithResponse(new Uri($"https://test.local/accounts/{accountNumber}"), _accountResponse)
                                                                            .WithResponse(new Uri($"https://test.local/accounts/{accountNumber}/balance"), _balanceResponse)
                                                                            .Build();
            var client = new HttpClient(stubHttpMessageHandler);
            var httpClient = client;
            httpClient.BaseAddress = new Uri("https://test.local");
            _mapper = Substitute.For<IMapper>();
            _mapper.Map<Account>(Arg.Any<FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank.Responses.Account>()).Returns(_expected);
            _mapper.Map(Arg.Any<FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank.Responses.Balance>(), _expected).Returns(_expected);

            _fairWayBankAccountProvider = new FairWayBankAccountProvider(httpClient, _mapper);
            _result = await _fairWayBankAccountProvider.TryGetAccount(accountNumber, new CancellationToken());
        }

        [Test]
        public void ReturnAValue() => _result.HasValue.Should().BeTrue();

        [Test]
        public void ReturnTheAccount() => _result.Value.Should().BeSameAs(_expected);

        [Test]
        public void MapTheAccount() => _mapper.Received(1).Map<Account>(Arg.Any<FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank.Responses.Account>());

        [Test]
        public void MapTheBalance() => _mapper.Received(1).Map(Arg.Any<FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank.Responses.Balance>(), _expected);

        [Test]
        public void HaveTheProviderNameFairWayBank() => _fairWayBankAccountProvider.Name.Should().Be("FairWayBank");
    }
}
