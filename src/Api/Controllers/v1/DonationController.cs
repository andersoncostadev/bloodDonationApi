using Application.Commands.v1.Donation.Create;
using Application.Commands.v1.Donation.Update;
using Application.Queries.Donation.GetAll;
using Application.Queries.Donation.GetDonationsReport;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DonationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateDonationAsync([FromQuery] Guid donorId, [FromBody] CreateDonationCommand command)
        {
            command.DonorId = donorId;

            var result = await _mediator.Send(command);

            if (result == null)
                return BadRequest("Failed to create donation");

            return Ok(result);
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDonationAsync([FromQuery] Guid donorId, [FromQuery] Guid id, [FromBody] UpdateDonationCommand command)
        {
            command.DonorId = donorId;
            command.Id = id;

            var result = await _mediator.Send(command);

            if (result == null)
                return BadRequest("Failed to update donation");

            return Ok(result);
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDonationsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllDonationQuery(pageSize, pageNumber));

            if (result == null)
                return BadRequest("Failed to get donations");

            return Ok(result);
        }

        [HttpGet("report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDonationsReportAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetDonationsReportQuery(pageNumber, pageSize));

            if (result == null)
                return BadRequest("Failed to get donations report");

            return Ok(result);
        }
    }
}
