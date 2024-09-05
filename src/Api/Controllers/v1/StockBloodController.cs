using Application.Queries.StockBlood.GetStockBloodReport;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockBloodController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockBloodController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStockBloodsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetStockBloodReportQuery(pageNumber, pageSize));

            if (response == null)
                return NotFound();

            return Ok(response);
        }
    }
}
