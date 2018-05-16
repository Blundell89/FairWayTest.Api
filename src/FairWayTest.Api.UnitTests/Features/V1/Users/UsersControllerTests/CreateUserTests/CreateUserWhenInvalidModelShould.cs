using System.Threading;
using System.Threading.Tasks;
using FairWayTest.Api.Features.V1.Users;
using FairWayTest.Api.Features.V1.Users.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users.UsersControllerTests
{
    public class CreateUserWhenInvalidModelShould
    {
        private BadRequestObjectResult _result;
        private string _errorMessage;

        [OneTimeSetUp]
        public async Task WhenCreatingUser()
        {
            var cancellationToken = new CancellationToken();

            var controller = new UsersController(null, null);
            _errorMessage = "something went wrong!";
            controller.ModelState.AddModelError("test", _errorMessage);
            _result = await controller.Create(new CreateUserRequest(), cancellationToken) as BadRequestObjectResult;
        }

        [Test]
        public void ReturnBadRequestStatusCode() => _result.StatusCode.Should().Be(400);

        [Test]
        public void ReturnErrorResponse() => _result.Value.Should().BeOfType<ErrorResponse>();

        [Test]
        public void ReturnErrorMessage() => ((ErrorResponse)_result.Value).Message.Should().BeEquivalentTo(_errorMessage);
    }
}