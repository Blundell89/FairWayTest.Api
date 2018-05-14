using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FairWayTest.Api.Features.V1.Users;
using FluentAssertions;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users.FindBankDetailsByAccountNumberTests
{
    public class FindBankDetailsByAccountNumberWhenNotExistsShould
    {
        private Maybe<BankDetails> _result;

        [OneTimeSetUp]
        public async Task WhenAFindQuerySent()
        {
            var cancellationToken = new CancellationToken();

            var database = Substitute.For<IMongoDatabase>();
            var collection = Substitute.For<IMongoCollection<User>>();
            var cursor = Substitute.For<IAsyncCursor<User>>();
            cursor.MoveNextAsync(cancellationToken).Returns(false);
            collection.FindAsync<User>(Arg.Any<Expression<Func<User, bool>>>(), null, cancellationToken).ReturnsForAnyArgs(cursor);
            database.GetCollection<User>("users").Returns(collection);

            var handler = new FindBankDetailsByAccountNumber(database);
            _result = await handler.Handle(new FindBankDetailsByAccountNumber.Query("12345678"), cancellationToken);
        }

        [Test]
        public void NotReturnAnyBankDetails() => _result.Value.Should().BeNull();

        [Test]
        public void ReturnAMaybeWithoutAValue() => _result.HasValue.Should().BeFalse();
    }
}