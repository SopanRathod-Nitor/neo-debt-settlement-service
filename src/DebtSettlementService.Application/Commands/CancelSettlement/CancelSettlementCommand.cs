using DebtSettlementService.Application.DTOs;
using MediatR;

namespace DebtSettlementService.Application.Commands.CancelSettlement;

public sealed record CancelSettlementCommand(Guid Id) : IRequest<SettlementDto>;
