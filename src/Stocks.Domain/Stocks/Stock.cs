using System;
using Stocks.Domain.Abstractions;

namespace Stocks.Domain.Stocks
{
    public class Stock : Entity<Guid>, IAggregateRoot
    {
        /// <inheritdoc />
        private Stock()
            : base(default)
        {
        }

        public Stock(string name)
            : this()
        {
            Name = name;
        }

        public string Name { get; private set; }

        public void ChangeName(string name)
        {
            Name = name;
        }
    }
}