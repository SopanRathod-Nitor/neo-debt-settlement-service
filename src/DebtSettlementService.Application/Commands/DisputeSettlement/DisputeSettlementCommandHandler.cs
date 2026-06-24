using DebtSettlementService.Application.Common;
using DebtSettlementService.Application.DTOs;
using DebtSettlementService.Application.Interfaces;
using MediatR;

namespace DebtSettlementService.Application.Commands.DisputeSettlement;

internal sealed class DisputeSettlementCommandHandler(ISettlementRepository repository)
    : IRequestHandler<DisputeSettlementCommand, SettlementDto>
{
    public async Task<SettlementDto> Handle(DisputeSettlementCommand request, CancellationToken ct)
    {
        var settlement = await repository.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"Settlement {request.Id} not found.");

        settlement.Dispute(request.Reason);
        await repository.UpdateAsync(settlement, ct);
        return SettlementMapper.ToDto(settlement);
    }
}
