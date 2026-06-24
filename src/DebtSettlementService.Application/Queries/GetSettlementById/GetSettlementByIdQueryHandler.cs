using DebtSettlementService.Application.Common;
using DebtSettlementService.Application.DTOs;
using DebtSettlementService.Application.Interfaces;
using MediatR;

namespace DebtSettlementService.Application.Queries.GetSettlementById;

internal sealed class GetSettlementByIdQueryHandler(ISettlementRepository repository)
    : IRequestHandler<GetSettlementByIdQuery, SettlementDto?>
{
    public async Task<SettlementDto?> Handle(GetSettlementByIdQuery request, CancellationToken ct)
    {
        var settlement = await repository.GetByIdAsync(request.Id, ct);
        return settlement is null ? null : SettlementMapper.ToDto(settlement);
    }
}
