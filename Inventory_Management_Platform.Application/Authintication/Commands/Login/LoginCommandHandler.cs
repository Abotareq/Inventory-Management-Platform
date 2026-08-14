using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Authentication;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Authentication;
using Inventory_Management_Platform.Domain.User.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Authintication.Commands.Login
{
    public sealed class LoginCommandHandler
        : IRequestHandler<LoginCommand, ErrorOr<LoginResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            IAuthService authService,
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _authService = authService;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<LoginResponse>> Handle(
            LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate credentials against Identity
            var credentialsResult = await _authService.ValidateCredentialsAsync(
                request.Email, request.Password);

            if (credentialsResult.IsError)
                return credentialsResult.Errors;

            var (userId, role) = credentialsResult.Value;

            // 2. Fetch the domain User (need FullName, and the domain object itself for token claims)
            var user = await _userRepository.GetByIdAsync(UserId.Create(userId));

            if (user is null)
                return Error.NotFound("Auth.UserNotFound", "User record not found.");

            // 3. Issue tokens
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, role);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            return new LoginResponse(
                user.UserId.Value,
                user.FullName,
                user.Email,
                role,
                accessToken,
                refreshToken);
        }
    }
}
