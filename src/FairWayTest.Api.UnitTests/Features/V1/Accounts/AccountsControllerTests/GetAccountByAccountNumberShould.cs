using System;
using System.Collections.Generic;
using System.Text;
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
    public class GetAccountByAccountNumberShould
    {
        private OkObjectResult _result;
        private Account _account;

        [OneTimeSetUp]
        public async Task WhenGettingAccountByAccountNumber()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            _account = new Account();

            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Is<GetUserAccount.Query>(x => x.UserId == userId), cancellationToken).Returns(Maybe<Account>.Some(_account));

            var controller = new AccountsController(mediator);
            _result = await controller.GetAccountByAccountNumber(userId, cancellationToken) as OkObjectResult;
        }

        [Test]
        public void ReturnOkStatusCode() => _result.StatusCode.Should().Be(200);

        [Test]
        public void ReturnTheAccount() => _result.Value.Should().BeSameAs(_account);
    }
}
