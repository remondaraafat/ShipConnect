using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

    #region Admin & Shipping Company

    [Authorize(Roles = "Admin")]
    [HttpGet("total-count")]
    public async Task<IActionResult> GetTotalShippingCompaniesCount()
    {
        var response = await _mediator.Send(new GetTotalShippingCompaniesCountQuery());
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var response = await _mediator.Send(new GetAllShippingCompaniesQuery(pageNumber, pageSize));
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _mediator.Send(new DeleteShippingCompanyCommand(id));
        return response.Success ? Ok(response) : NotFound(response);
    }

    [Authorize]
    [HttpGet("CompanyProfile/{companyId:int}")]
    public async Task<IActionResult> GeCompanyProfileById(int companyId)
    {
        var response = await _mediator.Send(new GetShippingCompanyByIdQuery(companyId, userId: null));
        return response.Success ? Ok(response) : NotFound(response);
    }

    [Authorize(Roles = "ShippingCompany")]
    [HttpGet("MyProfile")]
    public async Task<IActionResult> GeMyProfileById(int companyId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return Unauthorized();

        var response = await _mediator.Send(new GetShippingCompanyByIdQuery(null, userId));
        return response.Success ? Ok(response) : NotFound(response);
    }

    [Authorize(Roles = "ShippingCompany")]
    [HttpPut("updateProfile")]
    public async Task<IActionResult> Update([FromBody] CreateShippingCompanyDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return Unauthorized();

        var response = await _mediator.Send(new UpdateShippingCompanyCommand(userId, dto));
        return response.Success ? Ok(response) : NotFound(response);
    }

    #endregion



    [HttpGet("search")]
    public async Task<IActionResult> SearchByName([FromQuery] string name)
    {
        var response = await _mediator.Send(new SearchShippingCompaniesByNameQuery(name));
        return response.Success ? Ok(response) : NotFound(response);
    }
}
