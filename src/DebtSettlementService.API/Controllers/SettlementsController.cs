using Asp.Versioning;
using DebtSettlementService.Application.DTOs;
using DebtSettlementService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DebtSettlementService.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/settlements")]
[Authorize]
public sealed class SettlementsController(SettlementService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status, CancellationToken ct)
    {
        var list = await service.GetAllAsync(status, ct);
        return Ok(new { success = true, data = list, total = list.Count });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var s = await service.GetByIdAsync(id, ct);
        return s is null ? NotFound(new { success = false, message = $"Settlement {id} not found." })
                         : Ok(new { success = true, data = s });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSettlementRequest request, CancellationToken ct)
    {
        var s = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = s.Id, version = "1" }, new { success = true, data = s });
    }

    [HttpPut("{id:guid}/start")]
    public async Task<IActionResult> StartProcessing(Guid id, CancellationToken ct)
        => Ok(new { success = true, data = await service.StartProcessingAsync(id, ct) });

    [HttpPut("{id:guid}/settle")]
    public async Task<IActionResult> MarkSettled(Guid id, [FromBody] SettleRequest request, CancellationToken ct)
        => Ok(new { success = true, data = await service.MarkSettledAsync(id, request, ct) });

    [HttpPut("{id:guid}/dispute")]
    public async Task<IActionResult> Dispute(Guid id, [FromBody] DisputeRequest request, CancellationToken ct)
        => Ok(new { success = true, data = await service.DisputeAsync(id, request, ct) });

    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
        => Ok(new { success = true, data = await service.CancelAsync(id, ct) });
}
