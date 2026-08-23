using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Models
{
    public sealed class IdempotencyRecord
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = default!;
        public string RequestType { get; set; } = default!;
        public string ResponseData { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
