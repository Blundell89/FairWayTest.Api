using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FairWayTest.Api.Features.V1.Users;
using FluentAssertions;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Users
{
    public class GetUsersShould
    {
        private ICollection<User> _result;

        [OneTimeSetUp]
        public async Task WhenGettingExistingUsers()
        {
            var cancellationToken = new CancellationToken();

            var users = new[]
            {
                new User(), new User(), new User(),
            };

            var database = Substitute.For<IMongoDatabase>();
            var collection = Substitute.For<IMongoCollection<User>>();
            var cursor = Substitute.For<IAsyncCursor<User>>();
            cursor.Current.Returns(users);
            cursor.MoveNextAsync(cancellationToken).Returns(true, false);
            collection.FindAsync<User>(Arg.Any<Expression<Func<User, bool>>>(), null, cancellationToken).ReturnsForAnyArgs(cursor);
            database.GetCollection<User>("users").Returns(collection);
            
            var handler = new GetUsers(database);
            _result = await handler.Handle(new GetUsers.Query(), cancellationToken);
        }

        [Test]
        public void ReturnAllUsers() => _result.Count.Should().Be(3);
    }
}
