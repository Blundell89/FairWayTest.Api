using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FairWayTest.Api.Features.V1.Users.Requests;
using FluentAssertions;
using MongoDB.Driver;
using NUnit.Framework;

namespace FairWayTest.Api.FunctionalTests.Journeys.V1.Users.CreateAUserJourney
{
    public class CreateAUserWhenBadRequest
    {
        private CreateUserRequest _createUserRequest;
        private HttpResponseMessage _result;

        [OneTimeSetUp]
        public async Task GivenAUniqueBankAccount_WhenCreatingANewUser()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new AccountNumberBuilder());

            _createUserRequest = fixture.Build<CreateUserRequest>()
                                        .Without(x => x.FirstName)
                                        .Create();

            _result = await HttpClients.FairWayApi.PostAsJsonAsync("/users", _createUserRequest);
        }

        [Test]
        public void ThenTheRequestWasBad() => _result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        [Test]
        public async Task ThenAnErrorResponseIsReturned()
        {
            var errorResponse = await _result.Content.ReadAsAsync<ErrorResponse>();

            errorResponse.Message.Should().BeEquivalentTo("'First Name' should not be empty.");
        }

        [Test]
        public async Task ThenTheUserWasNotStored()
        {
            var user = await Database.Users.Find(x => x.Surname == _createUserRequest.Surname).SingleOrDefaultAsync();

            user.Should().BeNull();
        }
    }
}