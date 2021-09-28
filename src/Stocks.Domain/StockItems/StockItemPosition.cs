using System.Collections.Generic;
using Stocks.Domain.Abstractions;

namespace Stocks.Domain.StockItems
{
    public class StockItemPosition : ValueObject
    {
        /// <inheritdoc />
        public StockItemPosition(string number)
        {
            Number = number;
        }

        public string Number { get; private set; }
        
        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityFields()
        {
            yield return Number;
        }
    }
}