using DebtSettlementService.Application.Common;
using DebtSettlementService.Application.DTOs;
using DebtSettlementService.Application.Interfaces;
using MediatR;

namespace DebtSettlementService.Application.Commands.StartProcessing;

internal sealed class StartProcessingCommandHandler(ISettlementRepository repository)
    : IRequestHandler<StartProcessingCommand, SettlementDto>
{
    public async Task<SettlementDto> Handle(StartProcessingCommand request, CancellationToken ct)
    {
        var settlement = await repository.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"Settlement {request.Id} not found.");

        settlement.StartProcessing();
        await repository.UpdateAsync(settlement, ct);
        return SettlementMapper.ToDto(settlement);
    }
}
