using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FairWayTest.Api.Features.V1.Users
{
    [Route("[controller]")]
    [ApiVersion("1.0")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Requests.CreateUserRequest createUserRequest, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateUser.Command>(createUserRequest);

            var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

            if (result)
                return Created(new Uri($"users/{command.Id}", UriKind.Relative), command.Id);

            return BadRequest(new ErrorResponse(result.FailureReason));
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(new GetUsers.Query(), cancellationToken).ConfigureAwait(false);

            return Ok(users);
        }
    }
}