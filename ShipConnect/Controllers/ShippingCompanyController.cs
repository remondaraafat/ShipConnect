using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShipConnect.CQRS.ShippingCompanies.Queries;
using ShipConnect.DTOs.ShippingCompanies;
using ShipConnect.Helpers;
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
        var response = await _mediator.Send(new GetAllShippingCompaniesQuery());
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _mediator.Send(new GetShippingCompanyByIdQuery(id));
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShippingCompanyDto dto)
    {
        var response = await _mediator.Send(new CreateShippingCompanyCommand(dto));
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response)
            : BadRequest(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateShippingCompanyDto dto)
    {
        var response = await _mediator.Send(new UpdateShippingCompanyCommand(id, dto));
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _mediator.Send(new DeleteShippingCompanyCommand(id));
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpGet("total-count")]
    public async Task<IActionResult> GetTotalShippingCompaniesCount()
    {
        var response = await _mediator.Send(new GetTotalShippingCompaniesCountQuery());
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchByName([FromQuery] string name)
    {
        var response = await _mediator.Send(new SearchShippingCompaniesByNameQuery(name));
        return response.Success ? Ok(response) : NotFound(response);
    }
}
