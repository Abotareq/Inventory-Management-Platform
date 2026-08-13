using ErrorOr;
using Inventory_Management_Platform.Application.Authintication.Commands.Register;
using Inventory_Management_Platform.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_Platform.Api.Controllers
{

    [Route("api/auth")]
    public sealed class AuthenticationController : ApiController
    {
        private readonly ISender _mediator;

        public AuthenticationController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = new RegisterCommand(
                request.FullName,
                request.Email,
                request.Password,
                request.Role);

            ErrorOr<RegisterResponse> registerResult = await _mediator.Send(command);

            return registerResult.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
    }

}
