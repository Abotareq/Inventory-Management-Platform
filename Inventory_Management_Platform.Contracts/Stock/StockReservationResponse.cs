using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Stock
{
    public sealed record StockReservationResponse(
          Guid StockReservationId,
          Guid StockId,
          Guid OrderId,
          int Amount,
          string Action,
          Guid PerformedByUserId,
          DateTime Timestamp);
}
