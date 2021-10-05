using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stocks.Application.Commands.ChangeStock;
using Stocks.Application.Commands.CreateStock;
using Stocks.Application.Queries;
using Stocks.Application.Queries.Stocks;

namespace Stocks.Api.Controllers
{
    [ApiController]
    [Route("stocks")]
    public class StocksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStockQueries _stockQueries;

        /// <inheritdoc />
        public StocksController(IMediator mediator, IStockQueries stockQueries)
        {
            _mediator = mediator;
            _stockQueries = stockQueries;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageSize pageSize, CancellationToken cancellationToken)
        {
            var pagedCollection = await _stockQueries.GetAllAsync(pageSize, cancellationToken);

            return Ok(pagedCollection);
        }
        
        [HttpGet("{stockId:guid}")]
        public async Task<IActionResult> Get(Guid stockId, CancellationToken cancellationToken)
        {
            var stock = await _stockQueries.GetByIdAsync(stockId, cancellationToken);

            return Ok(stock);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateStockCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ChangeStockCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return Ok();
        }
    }
}