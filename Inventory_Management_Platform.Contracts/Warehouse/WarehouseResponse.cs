using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Warehouse
{
    public sealed record WarehouseResponse(
    Guid Id,
    string Name,
    string Location);
}
