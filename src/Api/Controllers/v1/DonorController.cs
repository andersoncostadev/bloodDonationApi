using Application.Commands.v1.Donor.Create;
using Application.Commands.v1.Donor.Delete;
using Application.Commands.v1.Donor.Update;
using Application.Queries.Donor.GetAll;
using Application.Queries.Donor.GetById;
using Application.Queries.Donor.GetFullName;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DonorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateDonorAsync([FromBody] CreateDonorCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDonorsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetAllDonorQuery(pageNumber, pageSize));

            if (response == null || !response.Donors!.Any())
                return NotFound();

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDonorByIdAsync(Guid id)
        {
            var query = new GetByIdDonorQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("fullname/{fullname}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDonorByFullNameAsync(string fullname)
        {
            var query = new GetByFullNameQuery(fullname);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDonorAsync([FromQuery] Guid donorId, [FromQuery] Guid id, [FromBody] UpdateDonorCommand command)
        {
            command.Id = donorId;
            command.Address!.Id = id;

            var result = await _mediator.Send(command);

            if (result == null)
                return BadRequest("Failed to update donor");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDonorAsync(Guid id)
        {
            var command = new DeleteDonorCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
