using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Warehouse
{
    public sealed record UpdateWarehouseRequest(
         string Name,
         string Location);
}
