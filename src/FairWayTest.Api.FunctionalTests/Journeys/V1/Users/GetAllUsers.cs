using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoFixture;
using FairWayTest.Api.Features.V1.Users;
using FluentAssertions;
using MongoDB.Driver;
using NUnit.Framework;

namespace FairWayTest.Api.FunctionalTests.Journeys.V1.Users
{
    public class GetAllUsers
    {
        private HttpResponseMessage _result;
        private User[] _users;

        [OneTimeSetUp]
        public async Task GivenRegisteredUsers_GettingAllUsers()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new AccountNumberBuilder());
            _users = fixture.CreateMany<User>().ToArray();

            await Database.Users.InsertManyAsync(_users);

            _result = await HttpClients.FairWayApi.GetAsync("/users");
        }

        [Test]
        public void ThenTheRequestWasSuccessful() => _result.StatusCode.Should().Be(HttpStatusCode.OK);

        [Test]
        public async Task ThenTheUsersWereRetrieved()
        {
            var users = await _result.Content.ReadAsAsync<User[]>();

            foreach (var user in users)
            {
                user.Should().BeEquivalentTo(_users.Single(x => x.Id == user.Id));
            }
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            var filter = Builders<User>.Filter.In(x => x.Id, _users.Select(u => u.Id));

            await Database.Users.DeleteManyAsync(filter);
        }
    }
}