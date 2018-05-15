using AutoFixture;
using AutoMapper;
using FairWayTest.Api.Features.V1.Users;
using FairWayTest.Api.Infrastructure.AccountProviders.Bizfibank;
using FairWayTest.Api.Infrastructure.AccountProviders.Bizfibank.Responses;
using FluentAssertions;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Infrastructure.AccountProviderTests.BizfiBank
{
    public class BizfiBankAccountResponseToAccountProfileShould
    {
        private IMapper _mapper;
        private Api.Features.V1.Accounts.Account _result;
        private Account _request;

        [OneTimeSetUp]
        public void WhenMapping()
        {
            _mapper = new MapperConfiguration(x => x.AddProfile<BizfiBankAccountToAccountProfile>()).CreateMapper();

            _request = new Fixture().Create<Account>();

            _result = _mapper.Map<FairWayTest.Api.Features.V1.Accounts.Account>(_request);
        }

        [Test]
        public void HaveAValidConfiguration()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Test]
        public void SuccessfullyMap()
        {
            _result.Should().BeEquivalentTo(_request);
        }
    }
}