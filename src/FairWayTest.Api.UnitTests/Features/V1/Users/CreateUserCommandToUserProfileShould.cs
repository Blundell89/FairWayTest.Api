using AutoFixture;
using AutoMapper;
using FairWayTest.Api.Features.V1.Users;
using FluentAssertions;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users
{
    public class CreateUserCommandToUserProfileShould
    {
        private IMapper _mapper;
        private User _result;
        private CreateUser.Command _request;

        [OneTimeSetUp]
        public void WhenMapping()
        {
            _mapper = new MapperConfiguration(x => x.AddProfile<CreateUserCommandToUserProfile>()).CreateMapper();

            _request = new Fixture().Create<CreateUser.Command>();

            _result = _mapper.Map<User>(_request);
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