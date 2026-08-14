using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Category
{
    public sealed record CreateCategoryRequest(
        string Name);
}
