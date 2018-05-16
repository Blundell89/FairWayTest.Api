using AutoFixture;
using AutoMapper;
using FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank;
using FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank.Responses;
using FluentAssertions;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Infrastructure.AccountProviderTests.FairWayBank
{
    public class FairWayBankAccountResponseToAccountProfileShould
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
                .With(x => x.Type, "Credit")
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
        public void SuccessfullyMapTheAccount()
        {
            _result.AccountNumber.Should().BeEquivalentTo(_accountRequest.Identifier.AccountNumber);
            _result.SortCode.Should().BeEquivalentTo(_accountRequest.Identifier.SortCode);
            _result.Name.Should().BeEquivalentTo(_accountRequest.Name);
        }

        [Test]
        public void SuccessfullyMapTheBalance()
        {
            _result.Overdraft.Value.Should().Be(_balanceRequest.Overdraft.Amount);
            _result.Balance.Should().Be(_balanceRequest.Amount);
            _result.AvailableBalance.Should().Be(_balanceRequest.Amount + _balanceRequest.Overdraft.Amount);
        }
    }
}