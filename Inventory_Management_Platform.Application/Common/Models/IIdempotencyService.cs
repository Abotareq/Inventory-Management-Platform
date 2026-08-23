using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Models
{
    public interface IIdempotencyService
    {
        Task<string?> GetStoredResponseAsync(string key, CancellationToken cancellationToken);
        Task StoreResponseAsync(string key, string requestType, string responseData, CancellationToken cancellationToken);
    }
}
