using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Order
{
    public sealed record OrderHistoryResponse(
        Guid OrderHistoryId,
        Guid OrderId,
        string FromStatus,
        string ToStatus,
        Guid PerformedByUserId,
        DateTime Timestamp);
}
