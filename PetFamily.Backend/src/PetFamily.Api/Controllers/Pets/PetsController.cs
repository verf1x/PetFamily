using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Controllers.Pets.Requests;
using PetFamily.Application.VolunteersManagement.Queries.GetFilteredPetsWithPagination;

namespace PetFamily.Api.Controllers.Pets;

public class PetsController : ApplicationController
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        [FromServices] GetFilteredPetsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        
        var response = await handler.HandleAsync(query, cancellationToken);

        return Ok(response); 
    }
    
    [HttpGet("dapper")]
    public async Task<IActionResult> GetAsync(
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        [FromServices] GetFilteredPetsWithPaginationHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        
        var response = await handler.HandleAsync(query, cancellationToken);

        return Ok(response); 
    }
}