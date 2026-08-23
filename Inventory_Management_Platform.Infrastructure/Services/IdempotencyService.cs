using Inventory_Management_Platform.Application.Common.Models;
using Inventory_Management_Platform.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace Inventory_Management_Platform.Infrastructure.Services
{
    public sealed class IdempotencyService : IIdempotencyService
    {
        private readonly InventoryManagementPlatformDbContext _dbContext;

        public IdempotencyService(InventoryManagementPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string?> GetStoredResponseAsync(string key, CancellationToken cancellationToken)
        {
            var record = await _dbContext.Set<IdempotencyRecord>()
                .FirstOrDefaultAsync(r => r.Key == key, cancellationToken);

            return record?.ResponseData;
        }

        public async Task StoreResponseAsync(
            string key, string requestType, string responseData, CancellationToken cancellationToken)
        {
            var record = new IdempotencyRecord
            {
                Id = Guid.NewGuid(),
                Key = key,
                RequestType = requestType,
                ResponseData = responseData,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Set<IdempotencyRecord>().AddAsync(record, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
