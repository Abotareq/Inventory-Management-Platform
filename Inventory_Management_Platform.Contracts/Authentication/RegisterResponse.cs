using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Authentication
{
    public sealed record RegisterResponse(
     Guid UserId,
     string FullName,
     string Email,
     string Role
    );
}
