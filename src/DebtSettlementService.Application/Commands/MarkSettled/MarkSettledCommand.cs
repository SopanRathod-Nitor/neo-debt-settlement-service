using DebtSettlementService.Application.DTOs;
using MediatR;

namespace DebtSettlementService.Application.Commands.MarkSettled;

public sealed record MarkSettledCommand(Guid Id, decimal SettledAmount, string Currency) : IRequest<SettlementDto>;
