using Microsoft.AspNetCore.Mvc;
using PetCareHub.Database;
using PetCareHub.Domain.Features.Pets;
using PetCareHub.Domain.Features.Pets.Models;

namespace PetCareHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private readonly IPetService _petService;

    public PetsController(IPetService petService)
    {
        _petService = petService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _petService.GetAllAsync(DbInitializer.DemoUserId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _petService.GetByIdAsync(DbInitializer.DemoUserId, id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePetRequest request)
    {
        var result = await _petService.CreateAsync(DbInitializer.DemoUserId, request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePetRequest request)
    {
        var result = await _petService.UpdateAsync(DbInitializer.DemoUserId, id, request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
