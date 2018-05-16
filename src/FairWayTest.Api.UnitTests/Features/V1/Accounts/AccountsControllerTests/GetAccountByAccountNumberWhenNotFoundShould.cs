using System;
using System.Threading;
using System.Threading.Tasks;
using FairWayTest.Api.Features.V1.Accounts;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace FairWayTest.Api.UnitTests.Features.V1.Accounts.AccountsControllerTests
{
    public class GetAccountByAccountNumberWhenNotFoundShould
    {
        private NotFoundResult _result;

        [OneTimeSetUp]
        public async Task WhenGettingAccountByAccountNumber()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Is<GetUserAccount.Query>(x => x.UserId == userId), cancellationToken).Returns(Maybe<Account>.None());

            var controller = new AccountsController(mediator);
            _result = await controller.GetAccountByAccountNumber(userId, cancellationToken) as NotFoundResult;
        }

        [Test]
        public void ReturnNotFoundStatusCode() => _result.StatusCode.Should().Be(404);
    }
}