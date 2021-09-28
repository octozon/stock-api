using System.Collections.Generic;
using Stocks.Domain.Abstractions;

namespace Stocks.Domain.StockItems
{
    public class StockItemState : ValueObject
    {
        public StockItemState()
        {
            CurrentState = "Initial";
        }

        public string CurrentState { get; set; }
        
        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityFields()
        {
            yield return CurrentState;
        }
    }
}