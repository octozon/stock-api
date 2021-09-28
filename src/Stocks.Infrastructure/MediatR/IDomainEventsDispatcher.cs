using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Infrastructure.MediatR
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchAllAsync(CancellationToken cancellationToken);
    }
}