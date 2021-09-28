using System;
using Stocks.Domain.Abstractions;

namespace Stocks.Domain.Stocks
{
    public interface IStockRepository : IRepository<Stock, Guid>
    {
        
    }
}