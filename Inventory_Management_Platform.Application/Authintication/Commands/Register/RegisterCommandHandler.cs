using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Authentication;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Authentication;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.User;
using Inventory_Management_Platform.Domain.User.Entites;
using Inventory_Management_Platform.Domain.User.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Authintication.Commands.Register
{
    public sealed class RegisterCommandHandler
   : IRequestHandler<RegisterCommand, ErrorOr<RegisterResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(
            IAuthService authService,
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<RegisterResponse>> Handle(
    RegisterCommand request, CancellationToken cancellationToken)
        {  // 1. Generate the shared id up front
            var userId = UserId.CreateUnique();

            // 2. Create the correct Domain aggregate based on the requested role
            //    (validates fullName/email before touching Identity)
            ErrorOr<User> userResult;

            switch (request.Role)
            {
                case "Administrator":
                    var adminResult = Administrator.Create(userId, request.FullName, request.Email);
                    userResult = adminResult.IsError ? adminResult.Errors : adminResult.Value;
                    break;

                case "WarehouseOperator":
                    var operatorResult = WarehouseOperator.Create(userId, request.FullName, request.Email);
                    userResult = operatorResult.IsError ? operatorResult.Errors : operatorResult.Value;
                    break;

                case "Manager":
                    var managerResult = Manager.Create(userId, request.FullName, request.Email);
                    userResult = managerResult.IsError ? managerResult.Errors : managerResult.Value;
                    break;
                case "SalesAgent":
                    var salesAgentResult = SalesAgent.Create(userId, request.FullName, request.Email);
                    userResult = salesAgentResult.IsError ? salesAgentResult.Errors : salesAgentResult.Value;
                    break;

                default:
                    userResult = Errors.User.InvalidRole;
                    break;
            }

            if (userResult.IsError)
                return userResult.Errors;

            var user = userResult.Value;

     

            // 3. Create the identity account using the SAME id and the requested role
            var identityResult = await _authService.RegisterIdentityUserAsync(
                userId.Value, request.Email, request.Password, request.Role);

            if (identityResult.IsError)
                return identityResult.Errors;

            var role = identityResult.Value;

            // 4. Persist the domain aggregate
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RegisterResponse(
                user.UserId.Value,
                user.FullName,
                user.Email,
                role
                );
        }
    }
}
