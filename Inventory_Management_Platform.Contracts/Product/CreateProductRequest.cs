using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Product
{
    public sealed record CreateProductRequest(
        string Name,
        string Sku,
        string? Description,
        Guid? CategoryId);
}
