using DebtSettlementService.Application.DTOs;
using MediatR;

namespace DebtSettlementService.Application.Queries.GetSettlementById;

public sealed record GetSettlementByIdQuery(Guid Id) : IRequest<SettlementDto?>;
