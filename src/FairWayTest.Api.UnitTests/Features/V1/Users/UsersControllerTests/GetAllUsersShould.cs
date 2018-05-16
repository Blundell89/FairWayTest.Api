using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FairWayTest.Api.Features.V1.Users;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users.UsersControllerTests
{
    public class GetAllUsersShould
    {
        private User[] _users;
        private OkObjectResult _result;

        [OneTimeSetUp]
        public async Task WhenGettingAllUsers()
        {
            var cancellationToken = new CancellationToken();
            _users = new[] {new User(), new User(),};

            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<GetUsers.Query>(), cancellationToken).Returns(_users);

            var controller = new UsersController(mediator, null);
            _result = await controller.Get(cancellationToken) as OkObjectResult;
        }

        [Test]
        public void ReturnOkStatusCode() => _result.StatusCode.Should().Be(200);

        [Test]
        public void ReturnAllUsers() => _result.Value.Should().BeSameAs(_users);
    }
}
