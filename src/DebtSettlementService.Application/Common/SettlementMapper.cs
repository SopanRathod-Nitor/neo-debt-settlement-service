using DebtSettlementService.Application.DTOs;
using DebtSettlementService.Domain.Entities;

namespace DebtSettlementService.Application.Common;

internal static class SettlementMapper
{
    internal static SettlementDto ToDto(Settlement s) => new(
        s.Id, s.DebtorName, s.DebtorAccountNumber, s.CreditorName,
        s.OriginalAmount.Amount, s.SettledAmount.Amount, s.OriginalAmount.Currency,
        s.Status.ToString(), s.PaymentMethod.ToString(),
        s.DueDate.ToString("yyyy-MM-dd"),
        s.SettledAt?.ToString("yyyy-MM-dd HH:mm:ss"),
        s.Notes,
        s.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
}
