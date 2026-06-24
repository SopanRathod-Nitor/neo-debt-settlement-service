using DebtSettlementService.Application.Common;
using DebtSettlementService.Application.DTOs;
using DebtSettlementService.Application.Interfaces;
using MediatR;

namespace DebtSettlementService.Application.Commands.CancelSettlement;

internal sealed class CancelSettlementCommandHandler(ISettlementRepository repository)
    : IRequestHandler<CancelSettlementCommand, SettlementDto>
{
    public async Task<SettlementDto> Handle(CancelSettlementCommand request, CancellationToken ct)
    {
        var settlement = await repository.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"Settlement {request.Id} not found.");

        settlement.Cancel();
        await repository.UpdateAsync(settlement, ct);
        return SettlementMapper.ToDto(settlement);
    }
}
