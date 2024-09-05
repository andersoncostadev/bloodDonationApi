using Application.Queries.Address.GetAll;
using Application.Queries.Address.GetPostalCode;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAddressesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetAllAddressQuery(pageNumber, pageSize));

            if (response == null || !response.Addresses!.Any())
                return NotFound();

            return Ok(response);
        }

        [HttpGet("{postalCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAddressByPostalCodeAsync(string postalCode)
        {
            var response = await _mediator.Send(new GetPostalCodeQuery(postalCode));

            if (response == null)
                return NotFound();

            return Ok(response);
        }

    }
}
