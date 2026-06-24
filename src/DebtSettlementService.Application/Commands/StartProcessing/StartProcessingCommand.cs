using DebtSettlementService.Application.DTOs;
using MediatR;

namespace DebtSettlementService.Application.Commands.StartProcessing;

public sealed record StartProcessingCommand(Guid Id) : IRequest<SettlementDto>;
