using DebtSettlementService.Application.DTOs;
using MediatR;

namespace DebtSettlementService.Application.Commands.DisputeSettlement;

public sealed record DisputeSettlementCommand(Guid Id, string Reason) : IRequest<SettlementDto>;
