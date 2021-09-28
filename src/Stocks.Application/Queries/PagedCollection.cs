using System;
using JetBrains.Annotations;

namespace Stocks.Application.Queries
{
    public class PagedCollection<TItem>
        where TItem : class
    {
        public PagedCollection(int size, int total, [NotNull] TItem[] items)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            if (total < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(total));
            }

            Items = items 
                ?? throw new ArgumentNullException(nameof(items));
            
            Pages = size != 0 ? (total + size - 1) / size : 0;
        }

        public TItem[] Items { get; private set; }

        public int Pages { get; private set; }
    }
}