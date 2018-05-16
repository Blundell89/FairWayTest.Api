using AutoFixture;
using AutoMapper;
using FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank;
using FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank.Responses;
using FluentAssertions;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Infrastructure.AccountProviderTests.FairWayBank
{
    public class FairWayBankAccountResponseToAccountProfileWhenDebitTypeShould
    {
        private IMapper _mapper;
        private Api.Features.V1.Accounts.Account _result;
        private Account _accountRequest;
        private Balance _balanceRequest;

        [OneTimeSetUp]
        public void WhenMapping()
        {
            _mapper = new MapperConfiguration(x => x.AddProfile<FairWayBankAccountToAccountProfile>()).CreateMapper();

            var fixture = new Fixture();
            _accountRequest = fixture.Create<Account>();
            _balanceRequest = fixture.Build<Balance>()
                .With(x => x.Type, "Debit")
                .Create();

            _result = _mapper.Map<FairWayTest.Api.Features.V1.Accounts.Account>(_accountRequest);
            _result = _mapper.Map(_balanceRequest, _result);
        }

        [Test]
        public void HaveAValidConfiguration()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Test]
        public void SuccessfullyMapTheBalance()
        {
            _result.Balance.Should().Be(-_balanceRequest.Amount);
        }
    }
}