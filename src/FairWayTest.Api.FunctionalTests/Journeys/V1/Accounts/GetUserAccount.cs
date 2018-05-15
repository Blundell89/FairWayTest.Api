using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FairWayTest.Api.Features.V1.Users;
using FluentAssertions;
using MongoDB.Driver;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace FairWayTest.Api.FunctionalTests.Journeys.V1.Accounts
{
    public class GetUserAccount
    {
        private const string _expectedResponse = "{\"account_name\": \"Current Account\",\"account_number\": \"12345678\",\"sort_code\": \"405431\",\"balance\": -8141.75,\"available_balance\": -7141.75,\"overdraft\": 1000}";
        private HttpResponseMessage _result;
        private FluentMockServer _fluentMockServer;
        private User _user;

        [OneTimeSetUp]
        public async Task GivenAnExistingUser_WhenGettingTheirAccountInformation()
        {
            var fixture = new Fixture();
            _user = fixture.Build<User>()
                .With(x => x.BankDetails, fixture.Build<BankDetails>()
                    .With(x => x.AccountNumber, "12345678")
                    .With(x => x.Name, "BizfiBank")
                    .Create())
                .Create();

            _fluentMockServer = FluentMockServer.Start(Configuration.BizfiBankUrl);
            _fluentMockServer.Given(Request.Create().WithPath($"/accounts/{_user.BankDetails.AccountNumber}").UsingGet())
                .RespondWith(Response.Create()
                                     .WithStatusCode(200)
                    .WithBody(_expectedResponse)
                    .WithHeader("Content-Type", "application/json"));


            await Database.Users.InsertOneAsync(_user);

            _result = await HttpClients.FairWayApi.GetAsync($"users/{_user.Id}/account");
        }

        [Test]
        public void ThenTheResponseIsValid() => _result.StatusCode.Should().Be(HttpStatusCode.OK);

        [Test]
        public async Task ThenTheAccountDetailsAreReturned()
        {
            var result = await _result.Content.ReadAsAsync<FairWayTest.Api.Features.V1.Accounts.Account>();

            result.AccountNumber.Should().Be("12345678");
            result.Name.Should().Be("Current Account");
            result.SortCode.Should().Be("405431");
            result.Balance.Should().Be(-8141.75m);
            result.AvailableBalance.Should().Be(-7141.75m);
            result.Overdraft.Should().Be(1000);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await Database.Users.DeleteOneAsync(x => x.Id == _user.Id);
            _fluentMockServer.Stop();
        } 
    }
}
