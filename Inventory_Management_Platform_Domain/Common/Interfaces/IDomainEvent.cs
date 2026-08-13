using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Common.Interfaces
{
    public interface IDomainEvent : INotification
    {
    }
}
