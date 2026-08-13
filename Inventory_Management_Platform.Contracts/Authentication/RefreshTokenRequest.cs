using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Authentication
{
    public sealed record RefreshTokenRequest(
    string AccessToken,
    string RefreshToken);
}
