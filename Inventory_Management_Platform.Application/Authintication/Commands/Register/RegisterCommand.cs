using ErrorOr;
using Inventory_Management_Platform.Contracts.Authentication;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Authintication.Commands.Register
{
    public sealed record RegisterCommand(
         string FullName,
         string Email,
         string Password,
         string Role) : IRequest<ErrorOr<RegisterResponse>>;
}
