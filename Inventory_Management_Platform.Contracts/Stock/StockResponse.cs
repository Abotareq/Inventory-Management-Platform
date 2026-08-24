using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Stock
{
    public sealed record StockResponse(
        Guid StockId,
        Guid ProductId,
        Guid WarehouseId,
        int Quantity,
        int Reserved,
        int Available);
}
