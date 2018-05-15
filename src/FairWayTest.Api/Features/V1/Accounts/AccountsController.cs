using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FairWayTest.Api.Features.V1.Accounts
{
    [ApiVersion("1.0")]
    public class AccountsController : Controller
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("users/{userId:guid}/account")]
        public async Task<IActionResult> GetAccountByAccountNumber(Guid userId, CancellationToken cancellationToken)
        {
            var accountMaybe = await _mediator.Send(new GetUserAccount.Query(userId), cancellationToken).ConfigureAwait(false);

            if (accountMaybe.HasValue)
            {
                return Ok(accountMaybe.Value);
            }

            return NotFound();
        }
    }
}
