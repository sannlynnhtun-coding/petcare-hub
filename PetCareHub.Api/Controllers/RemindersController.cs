using Microsoft.AspNetCore.Mvc;
using PetCareHub.Domain.Features.Reminders;
using PetCareHub.Domain.Features.Reminders.Models;

namespace PetCareHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RemindersController : ControllerBase
{
    private readonly IReminderService _reminderService;
    private static readonly Guid DemoUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public RemindersController(IReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming()
    {
        var result = await _reminderService.GetUpcomingAsync(DemoUserId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _reminderService.GetPendingAsync(DemoUserId);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetForEdit(Guid id)
    {
        var result = await _reminderService.GetPendingForEditAsync(DemoUserId, id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ReminderEditFields request)
    {
        var result = await _reminderService.UpdatePendingAsync(DemoUserId, id, request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReminderRequest request)
    {
        var result = await _reminderService.CreateAsync(DemoUserId, request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        var result = await _reminderService.CompleteAsync(DemoUserId, id);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
