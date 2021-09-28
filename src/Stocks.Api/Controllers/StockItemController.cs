using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stocks.Application.Commands.CreateStockItem;
using Stocks.Application.Commands.MoveStockItem;
using Stocks.Application.Queries;
using Stocks.Application.Queries.StockItems;

namespace Stocks.Api.Controllers
{
    [ApiController]
    [Route("stock_items")]
    public class StockItemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStockItemsQueries _stockItemsQueries;

        public StockItemController(IMediator mediator, IStockItemsQueries stockItemsQueries)
        {
            _mediator = mediator;
            _stockItemsQueries = stockItemsQueries;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageSize pageSize, CancellationToken cancellationToken)
        {
            PagedCollection<StockItemView> items = await _stockItemsQueries.GetAllAsync(pageSize, cancellationToken);

            return Ok(items);
        }
        
        [HttpGet("{stockItemId:guid}")]
        public async Task<IActionResult> Get(Guid stockItemId, CancellationToken cancellationToken)
        {
            StockItemDetailedView item = await _stockItemsQueries.GetByIdAsync(stockItemId, cancellationToken);

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateStockItemCommand command, CancellationToken cancellationToken)
        {
            CreateStockItemResult result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                actionName: nameof(Get),
                routeValues: new { stockItemId = result.StockItemId },
                value: result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(MoveStockItemCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return Ok();
        }
    }
}