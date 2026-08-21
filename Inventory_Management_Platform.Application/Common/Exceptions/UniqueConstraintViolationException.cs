using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Exceptions
{

    public sealed class UniqueConstraintViolationException : Exception
    {
        public UniqueConstraintViolationException(string message) : base(message) { }
    }

    public sealed class ConcurrencyConflictException : Exception
    {
        public ConcurrencyConflictException(string message) : base(message) { }
    }
}
