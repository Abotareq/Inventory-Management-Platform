using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Order.Enums
{
    public enum OrderStatus
    {
        Draft = 0,
        Submitted = 1,
        Processing = 2,
        Completed = 3,
        Cancelled = 4
    }
}
