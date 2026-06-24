namespace DebtSettlementService.Application.DTOs;

public sealed record SettlementDto(
    Guid Id, string DebtorName, string DebtorAccountNumber, string CreditorName,
    decimal OriginalAmount, decimal SettledAmount, string Currency,
    string Status, string PaymentMethod, string DueDate, string? SettledAt, string? Notes, string CreatedAt);

public sealed record CreateSettlementRequest(
    string DebtorName, string DebtorAccountNumber, string CreditorName,
    decimal OriginalAmount, string Currency, string PaymentMethod, string DueDate, string? Notes);

public sealed record SettleRequest(decimal SettledAmount, string Currency);

public sealed record DisputeRequest(string Reason);
