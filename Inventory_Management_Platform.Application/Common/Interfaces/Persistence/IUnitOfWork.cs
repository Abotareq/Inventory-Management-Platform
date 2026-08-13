using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Interfaces.Authentication
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
