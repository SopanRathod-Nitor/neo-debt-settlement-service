using DebtSettlementService.Application.DTOs;
using MediatR;

namespace DebtSettlementService.Application.Queries.GetAllSettlements;

public sealed record GetAllSettlementsQuery(string? Status) : IRequest<IReadOnlyList<SettlementDto>>;
