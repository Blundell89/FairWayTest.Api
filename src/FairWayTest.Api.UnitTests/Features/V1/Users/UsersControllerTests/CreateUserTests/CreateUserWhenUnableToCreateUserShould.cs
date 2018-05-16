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

namespace FairWayTest.Api.UnitTests.Features.V1.Users.UsersControllerTests
{
    public class CreateUserWhenUnableToCreateUserShould
    {
        private BadRequestObjectResult _result;
        private string _errorMessage;

        [OneTimeSetUp]
        public async Task WhenCreatingUser()
        {
            var cancellationToken = new CancellationToken();
            var request = new CreateUserRequest();
            var command = new CreateUser.Command();
            _errorMessage = "error!";

            var mapper = Substitute.For<IMapper>();
            mapper.Map<CreateUser.Command>(request).Returns(command);

            var mediator = Substitute.For<IMediator>();
            mediator.Send(command, cancellationToken).Returns(CommandResult.Fail(_errorMessage));

            var controller = new UsersController(mediator, mapper);
            _result = await controller.Create(request, cancellationToken) as BadRequestObjectResult;
        }

        [Test]
        public void ReturnBadRequestStatusCode() => _result.StatusCode.Should().Be(400);

        [Test]
        public void ReturnErrorResponse() => _result.Value.Should().BeOfType<ErrorResponse>();

        [Test]
        public void ReturnErrorMessage() => ((ErrorResponse)_result.Value).Message.Should().BeEquivalentTo(_errorMessage);
    }
}