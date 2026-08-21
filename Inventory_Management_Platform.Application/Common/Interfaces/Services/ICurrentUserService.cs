using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Interfaces.Services
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
    }
}
