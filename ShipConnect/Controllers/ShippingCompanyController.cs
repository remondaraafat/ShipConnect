using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.ShippingCompanies.Queries;
using ShipConnect.DTOs.ShippingCompanies;
using ShipConnect.ShippingCompanies.Commands;
using ShipConnect.ShippingCompanies.Querys;

[ApiController]
[Route("api/[controller]")]
public class ShippingCompanyController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShippingCompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllShippingCompaniesQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetShippingCompanyByIdQuery(id));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShippingCompanyDto dto)
    {
        var result = await _mediator.Send(new CreateShippingCompanyCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateShippingCompanyDto dto)
    {
        var result = await _mediator.Send(new UpdateShippingCompanyCommand(id, dto));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _mediator.Send(new DeleteShippingCompanyCommand(id));
        return success ? NoContent() : NotFound();
    }

    [HttpGet("total-count")]
    public async Task<IActionResult> GetTotalShippingCompaniesCount()
    {
        var count = await _mediator.Send(new GetTotalShippingCompaniesCountQuery());
        return Ok(new { totalCompanies = count });
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchByName([FromQuery] string name)
    {
        var result = await _mediator.Send(new SearchShippingCompaniesByNameQuery(name));
        return Ok(result);
    }


}
