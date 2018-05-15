using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FairWayTest.Api.Features.V1.Accounts;
using FairWayTest.Api.Features.V1.Users;
using FairWayTest.Api.Infrastructure;
using FluentAssertions;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Accounts
{
    public class GetUserAccountShould
    {
        private Maybe<Account> _result;
        private Account _account;

        [OneTimeSetUp]
        public async Task WhenGettingUserAccount()
        {
            var user = new User
            {
                BankDetails = new BankDetails
                {
                    AccountNumber = "123",
                    Name = "Bank1"
                }
            };

            _account = new Account();

            var accountProvider = Substitute.For<IAccountProvider>();
            accountProvider.Name.Returns("Bank1");
            accountProvider.TryGetAccount(user.BankDetails.AccountNumber, CancellationToken.None).Returns(Maybe<Account>.Some(_account));

            var database = Substitute.For<IMongoDatabase>();
            var collection = Substitute.For<IMongoCollection<User>>();
            var cursor = Substitute.For<IAsyncCursor<User>>();
            cursor.Current.Returns(new[] {user});
            cursor.MoveNextAsync(CancellationToken.None).Returns(true, false);
            collection.FindAsync<User>(Arg.Any<Expression<Func<User, bool>>>(), null, CancellationToken.None).ReturnsForAnyArgs(cursor);
            database.GetCollection<User>("users").Returns(collection);

            var handler = new GetUserAccount(new[] {accountProvider}, database);
            _result = await handler.Handle(new GetUserAccount.Query(user.Id), CancellationToken.None);
        }

        [Test]
        public void HaveAValue() => _result.HasValue.Should().BeTrue();

        [Test]
        public void ReturnTheAccount() => _result.Value.Should().BeSameAs(_account);
    }
}
