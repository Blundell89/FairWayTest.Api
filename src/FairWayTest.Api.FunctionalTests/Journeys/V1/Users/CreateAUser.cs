using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FairWayTest.Api.Features.V1.Users.Requests;
using FluentAssertions;
using MongoDB.Driver;
using NUnit.Framework;

namespace FairWayTest.Api.FunctionalTests.Journeys.V1.Users
{
    public class CreateAUser
    {
        private CreateUserRequest _createUserRequest;
        private HttpResponseMessage _result;

        [OneTimeSetUp]
        public async Task GivenAUniqueBankAccount_WhenCreatingANewUser()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new AccountNumberBuilder());

            _createUserRequest = fixture.Create<CreateUserRequest>();

            _result = await HttpClients.FairWayApi.PostAsJsonAsync("/users", _createUserRequest);
        }

        [Test]
        public void ThenTheRequestWasSuccessful() => _result.StatusCode.Should().Be(HttpStatusCode.Created);

        [Test]
        public void ThenTheLocationWasSet() => _result.Headers.Location.Should().NotBeNull();

        [Test]
        public async Task ThenTheUserWasStored()
        {
            var userId = await _result.Content.ReadAsAsync<Guid>();

            var user = await Database.Users.Find(x => x.Id == userId).SingleAsync();

            user.Should().BeEquivalentTo(_createUserRequest);
        }

        [OneTimeTearDown]
        public async Task TearDown() => await Database.Users.DeleteOneAsync(x => x.BankDetails.Name == _createUserRequest.BankDetails.Name);
    }
}
