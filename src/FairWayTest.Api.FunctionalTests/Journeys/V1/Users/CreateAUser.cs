using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
            _createUserRequest = new CreateUserRequest
            {
                BankDetails = new BankDetails
                {
                    AccountNumber = "12345678",
                    Name = "BizfiBank"
                }
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Api-Version", "1.0");
            client.BaseAddress = new Uri(Configuration.BaseUrl, UriKind.Absolute);

            _result = await client.PostAsJsonAsync("/users", _createUserRequest);
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
