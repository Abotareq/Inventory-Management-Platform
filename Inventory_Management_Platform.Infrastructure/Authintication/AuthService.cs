using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Authentication;
using Inventory_Management_Platform.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Authintication
{
    public sealed class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ErrorOr<string>> RegisterIdentityUserAsync(
            Guid userId, string email, string password, string role)
        {
            var existing = await _userManager.FindByEmailAsync(email);
            if (existing is not null)
                return Error.Conflict("Auth.EmailAlreadyExists", "An account with this email already exists.");

            var identityUser = new ApplicationUser
            {
                Id = userId,
                UserName = email,
                Email = email
            };

            var createResult = await _userManager.CreateAsync(identityUser, password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList();
                return errors;
            }

            await _userManager.AddToRoleAsync(identityUser, role);

            return role;
        }

        public async Task<ErrorOr<(Guid UserId, string Role)>> ValidateCredentialsAsync(
            string email, string password)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);
            if (identityUser is null)
                return Error.Unauthorized("Auth.InvalidCredentials", "Invalid email or password.");

            var passwordValid = await _userManager.CheckPasswordAsync(identityUser, password);
            if (!passwordValid)
                return Error.Unauthorized("Auth.InvalidCredentials", "Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(identityUser);
            var role = roles.FirstOrDefault();
            if (role is null)
                return Error.Unauthorized("Auth.NoRoleAssigned", "This account has no assigned role.");

            return (identityUser.Id, role);
        }

        public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryTime)
        {
            var identityUser = await _userManager.FindByIdAsync(userId.ToString());
            if (identityUser is null) return;

            identityUser.RefreshToken = refreshToken;
            identityUser.RefreshTokenExpiryTime = expiryTime;
            await _userManager.UpdateAsync(identityUser);
        }

        public async Task<ErrorOr<string>> ValidateStoredRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var identityUser = await _userManager.FindByIdAsync(userId.ToString());

            if (identityUser is null ||
                identityUser.RefreshToken != refreshToken ||
                identityUser.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Error.Unauthorized("Auth.InvalidRefreshToken", "Refresh token is invalid or expired.");
            }

            var roles = await _userManager.GetRolesAsync(identityUser);
            var role = roles.FirstOrDefault();
            if (role is null)
                return Error.Unauthorized("Auth.NoRoleAssigned", "This account has no assigned role.");

            return role;
        }

        public Task<ErrorOr<string>> RegisterIdentityUserAsync(Guid userId, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
