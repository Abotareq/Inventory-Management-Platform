using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Models

{
    public sealed class AuditLog
    {
        public Guid Id { get; set; }
        public string EntityName { get; set; } = default!;
        public string EntityId { get; set; } = default!;
        public string Action { get; set; } = default!; // Created, Modified, Deleted
        public string? Changes { get; set; } // JSON: { "PropertyName": { "old": ..., "new": ... } }
        public Guid? PerformedByUserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
