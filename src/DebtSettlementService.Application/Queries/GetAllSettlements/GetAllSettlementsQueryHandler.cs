using DebtSettlementService.Application.Common;
using DebtSettlementService.Application.DTOs;
using DebtSettlementService.Application.Interfaces;
using DebtSettlementService.Domain.Enums;
using MediatR;

namespace DebtSettlementService.Application.Queries.GetAllSettlements;

internal sealed class GetAllSettlementsQueryHandler(ISettlementRepository repository)
    : IRequestHandler<GetAllSettlementsQuery, IReadOnlyList<SettlementDto>>
{
    public async Task<IReadOnlyList<SettlementDto>> Handle(GetAllSettlementsQuery request, CancellationToken ct)
    {
        if (request.Status is not null && Enum.TryParse<SettlementStatus>(request.Status, true, out var parsed))
            return (await repository.GetByStatusAsync(parsed, ct)).Select(SettlementMapper.ToDto).ToList();

        return (await repository.GetAllAsync(ct)).Select(SettlementMapper.ToDto).ToList();
    }
}
