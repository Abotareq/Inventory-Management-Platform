using ErrorOr;
using Inventory_Management_Platform.Contracts.Authentication;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Authintication.Commands.Login
{
    public sealed record LoginCommand(
   string Email,
   string Password) : IRequest<ErrorOr<LoginResponse>>;
}
