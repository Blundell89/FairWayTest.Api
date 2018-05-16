using System;
using System.Net;
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
    public class BizfiBankAccountProviderWhenAccountNotFoundShould
    {
        private Maybe<Account> _result;
        private BizfiBankAccountProvider _bizfiBankAccountProvider;
        private const string _response = "{\"message\": \"Unable to find account with account number '12345678'\",\"status\": 404,\"errorCode\": 1001123}";

        [OneTimeSetUp]
        public async Task WhenGettingAccount()
        {
            var accountNumber = "12345678";

            var stubHttpMessageHandler = new StubHttpMessageHandlerBuilder().WithResponse(new Uri($"https://test.local/accounts/{accountNumber}"), _response, HttpStatusCode.NotFound)
                                                                            .Build();
            var httpClient = new HttpClient(stubHttpMessageHandler) {BaseAddress = new Uri("https://test.local")};
            var mapper = Substitute.For<IMapper>();

            _bizfiBankAccountProvider = new BizfiBankAccountProvider(httpClient, mapper);
            _result = await _bizfiBankAccountProvider.TryGetAccount(accountNumber, new CancellationToken());
        }

        [Test]
        public void NotReturnAValue() => _result.HasValue.Should().BeFalse();
    }
}