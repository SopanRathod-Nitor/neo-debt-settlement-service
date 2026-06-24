using DebtSettlementService.Application.DTOs;
using MediatR;

namespace DebtSettlementService.Application.Commands.CreateSettlement;

public sealed record CreateSettlementCommand(
    string DebtorName,
    string DebtorAccountNumber,
    string CreditorName,
    decimal OriginalAmount,
    string Currency,
    string PaymentMethod,
    string DueDate,
    string? Notes) : IRequest<SettlementDto>;
