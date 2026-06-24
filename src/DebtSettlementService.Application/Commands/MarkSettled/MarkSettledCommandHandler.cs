using DebtSettlementService.Application.Common;
using DebtSettlementService.Application.DTOs;
using DebtSettlementService.Application.Interfaces;
using DebtSettlementService.Domain.ValueObjects;
using MediatR;

namespace DebtSettlementService.Application.Commands.MarkSettled;

internal sealed class MarkSettledCommandHandler(ISettlementRepository repository)
    : IRequestHandler<MarkSettledCommand, SettlementDto>
{
    public async Task<SettlementDto> Handle(MarkSettledCommand request, CancellationToken ct)
    {
        var settlement = await repository.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"Settlement {request.Id} not found.");

        settlement.MarkSettled(new Money(request.SettledAmount, request.Currency));
        await repository.UpdateAsync(settlement, ct);
        return SettlementMapper.ToDto(settlement);
    }
}
