using System.Collections.Generic;
using MediatR;

namespace Stocks.Domain.Abstractions
{
    public interface IHaveEvents
    {
        Queue<INotification> DomainEvents { get; }
    }
}