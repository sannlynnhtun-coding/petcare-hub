using Microsoft.AspNetCore.Mvc;
using PetCareHub.Domain.Features.Pets;
using PetCareHub.Domain.Features.Pets.Models;

namespace PetCareHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private readonly IPetService _petService;

    // For MVP we use a fixed demo user. In real app use JWT claims.
    private static readonly Guid DemoUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public PetsController(IPetService petService)
    {
        _petService = petService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _petService.GetAllAsync(DemoUserId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _petService.GetByIdAsync(DemoUserId, id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePetRequest request)
    {
        var result = await _petService.CreateAsync(DemoUserId, request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePetRequest request)
    {
        var result = await _petService.UpdateAsync(DemoUserId, id, request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
