using Asp.Versioning;
using DebtSettlementService.Application.Commands.CancelSettlement;
using DebtSettlementService.Application.Commands.CreateSettlement;
using DebtSettlementService.Application.Commands.DisputeSettlement;
using DebtSettlementService.Application.Commands.MarkSettled;
using DebtSettlementService.Application.Commands.StartProcessing;
using DebtSettlementService.Application.DTOs;
using DebtSettlementService.Application.Queries.GetAllSettlements;
using DebtSettlementService.Application.Queries.GetSettlementById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DebtSettlementService.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/settlements")]
[Authorize]
public sealed class SettlementsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status, CancellationToken ct)
    {
        var list = await mediator.Send(new GetAllSettlementsQuery(status), ct);
        return Ok(new { success = true, data = list, total = list.Count });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSettlementByIdQuery(id), ct);
        return result is null
            ? NotFound(new { success = false, message = $"Settlement {id} not found." })
            : Ok(new { success = true, data = result });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSettlementRequest request, CancellationToken ct)
    {
        var command = new CreateSettlementCommand(
            request.DebtorName, request.DebtorAccountNumber, request.CreditorName,
            request.OriginalAmount, request.Currency, request.PaymentMethod,
            request.DueDate, request.Notes);

        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id, version = "1" },
            new { success = true, data = result });
    }

    [HttpPut("{id:guid}/start")]
    public async Task<IActionResult> StartProcessing(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new StartProcessingCommand(id), ct);
        return Ok(new { success = true, data = result });
    }

    [HttpPut("{id:guid}/settle")]
    public async Task<IActionResult> MarkSettled(Guid id, [FromBody] SettleRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new MarkSettledCommand(id, request.SettledAmount, request.Currency), ct);
        return Ok(new { success = true, data = result });
    }

    [HttpPut("{id:guid}/dispute")]
    public async Task<IActionResult> Dispute(Guid id, [FromBody] DisputeRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new DisputeSettlementCommand(id, request.Reason), ct);
        return Ok(new { success = true, data = result });
    }

    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new CancelSettlementCommand(id), ct);
        return Ok(new { success = true, data = result });
    }
}
