using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using AutoMapper;
using FairWayTest.Api.Features.V1.Users;
using FairWayTest.Api.Features.V1.Users.Requests;
using FluentAssertions;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users
{
    public class CreateUserRequestToCreateUserCommandProfileShould
    {
        private IMapper _mapper;
        private CreateUser.Command _result;
        private CreateUserRequest _request;

        [OneTimeSetUp]
        public void WhenMapping()
        {
            _mapper = new MapperConfiguration(x => x.AddProfile<CreateUserRequestToCreateUserCommandProfile>()).CreateMapper();

            _request = new Fixture().Create<CreateUserRequest>();

            _result = _mapper.Map<CreateUser.Command>(_request);
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

        [Test]
        public void GenerateAUniqueId()
        {
            _result.Id.Should().NotBeEmpty();
        }
    }
}
