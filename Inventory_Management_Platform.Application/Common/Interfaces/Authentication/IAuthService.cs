using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Interfaces.Authentication
{
    public interface IAuthService
    {

        Task<ErrorOr<string>> RegisterIdentityUserAsync(Guid userId, string email, string password, string role);
        Task<ErrorOr<(Guid UserId, string Role)>> ValidateCredentialsAsync(string email, string password);
        Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryTime);
        Task<ErrorOr<string>> ValidateStoredRefreshTokenAsync(Guid userId, string refreshToken);
    }
}
