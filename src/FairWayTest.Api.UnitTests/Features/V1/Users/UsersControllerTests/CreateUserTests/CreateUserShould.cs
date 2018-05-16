using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FairWayTest.Api.Features.V1.Users;
using FairWayTest.Api.Features.V1.Users.Requests;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users.UsersControllerTests.CreateUserTests
{
    public class CreateUserShould
    {
        private CreatedResult _result;
        private Guid _expectedId;

        [OneTimeSetUp]
        public async Task WhenCreatingUser()
        {
            var cancellationToken = new CancellationToken();
            var request = new CreateUserRequest();
            _expectedId = Guid.NewGuid();
            var command = new CreateUser.Command
            {
                Id = _expectedId
            };

            var mapper = Substitute.For<IMapper>();
            mapper.Map<CreateUser.Command>(request).Returns(command);

            var mediator = Substitute.For<IMediator>();
            mediator.Send(command, cancellationToken).Returns(CommandResult.Success);

            var controller = new UsersController(mediator, mapper);
            _result = await controller.Create(request, cancellationToken) as CreatedResult;
        }

        [Test]
        public void ReturnCreatedStatusCode() => _result.StatusCode.Should().Be(201);

        [Test]
        public void ReturnCreatedId() => ((Guid)_result.Value).Should().Be(_expectedId);

        [Test]
        public void SetTheLocationHeader() => _result.Location.Should().Be($"users/{_expectedId}");
    }
}