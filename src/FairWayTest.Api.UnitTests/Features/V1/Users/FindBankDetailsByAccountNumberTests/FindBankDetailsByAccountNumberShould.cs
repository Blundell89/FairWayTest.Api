using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FairWayTest.Api.Features.V1.Users;
using FluentAssertions;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users.FindBankDetailsByAccountNumberTests
{
    public class FindBankDetailsByAccountNumberShould
    {
        private BankDetails _bankDetails;
        private Maybe<BankDetails> _result;

        [OneTimeSetUp]
        public async Task WhenAFindQuerySent()
        {
            var cancellationToken = new CancellationToken();

            _bankDetails = new BankDetails
            {
                AccountNumber = "12345678"
            };
            var user = new User
            {
                BankDetails = _bankDetails
            };

            var database = Substitute.For<IMongoDatabase>();
            var collection = Substitute.For<IMongoCollection<User>>();
            var cursor = Substitute.For<IAsyncCursor<User>>();
            cursor.Current.Returns(new[] {user});
            cursor.MoveNextAsync(cancellationToken).Returns(true, false);
            collection.FindAsync<User>(Arg.Any<Expression<Func<User, bool>>>(), null, cancellationToken).ReturnsForAnyArgs(cursor);
            database.GetCollection<User>("users").Returns(collection);

            var handler = new FindBankDetailsByAccountNumber(database);
            _result = await handler.Handle(new FindBankDetailsByAccountNumber.Query(_bankDetails.AccountNumber), cancellationToken);
        }

        [Test]
        public void ReturnTheBankDetails() => _result.Value.Should().BeSameAs(_bankDetails);

        [Test]
        public void ReturnAMaybeWithAValue() => _result.HasValue.Should().BeTrue();
    }
}
