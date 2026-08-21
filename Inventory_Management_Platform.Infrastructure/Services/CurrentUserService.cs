using Inventory_Management_Platform.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
namespace Inventory_Management_Platform.Infrastructure.Services
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var sub = _httpContextAccessor.HttpContext?.User
                    .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                return sub is not null && Guid.TryParse(sub, out var id) ? id : null;
            }
        }
    }
}
