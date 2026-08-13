using Inventory_Management_Platform.Domain.User;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(User user, string role);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken);
    }
}
