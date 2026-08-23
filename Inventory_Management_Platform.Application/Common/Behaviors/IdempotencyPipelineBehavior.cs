using Inventory_Management_Platform.Application.Common.Interfaces.Services;
using Inventory_Management_Platform.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Inventory_Management_Platform.Application.Common.Behaviors
{
    public sealed class IdempotencyPipelineBehavior<TRequest, TResponse>
           : IPipelineBehavior<TRequest, TResponse>
           where TRequest : notnull
    {
        private readonly IIdempotencyService _idempotencyService;

        public IdempotencyPipelineBehavior(IIdempotencyService idempotencyService)
        {
            _idempotencyService = idempotencyService;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (request is not IIdempotentRequest idempotentRequest
                || string.IsNullOrWhiteSpace(idempotentRequest.IdempotencyKey))
            {
                return await next();
            }

            var requestType = typeof(TRequest).Name;

            var storedResponse = await _idempotencyService.GetStoredResponseAsync(
                idempotentRequest.IdempotencyKey, cancellationToken);

            if (storedResponse is not null)
            {
                return JsonSerializer.Deserialize<TResponse>(storedResponse)!;
            }

            var response = await next();

            var serialized = JsonSerializer.Serialize(response);
            await _idempotencyService.StoreResponseAsync(
                idempotentRequest.IdempotencyKey, requestType, serialized, cancellationToken);

            return response;
        }
    }
}
