using DebtSettlementService.Domain.Enums;
using DebtSettlementService.Domain.ValueObjects;

namespace DebtSettlementService.Domain.Entities;

public sealed class Settlement
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string DebtorName { get; private set; } = string.Empty;
    public string DebtorAccountNumber { get; private set; } = string.Empty;
    public string CreditorName { get; private set; } = string.Empty;
    public Money OriginalAmount { get; private set; } = Money.Zero;
    public Money SettledAmount { get; private set; } = Money.Zero;
    public SettlementStatus Status { get; private set; } = SettlementStatus.Pending;
    public PaymentMethod PaymentMethod { get; private set; }
    public DateOnly DueDate { get; private set; }
    public DateTime? SettledAt { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    private Settlement() { }

    public static Settlement Create(
        string debtorName, string debtorAccountNumber, string creditorName,
        Money originalAmount, PaymentMethod paymentMethod, DateOnly dueDate, string? notes = null)
    {
        return new Settlement
        {
            DebtorName = debtorName, DebtorAccountNumber = debtorAccountNumber,
            CreditorName = creditorName, OriginalAmount = originalAmount,
            SettledAmount = Money.Zero, PaymentMethod = paymentMethod,
            DueDate = dueDate, Notes = notes,
        };
    }

    public void StartProcessing()
    {
        if (Status != SettlementStatus.Pending)
            throw new InvalidOperationException("Only Pending settlements can be started.");
        Status = SettlementStatus.InProgress; UpdatedAt = DateTime.UtcNow;
    }

    public void MarkSettled(Money settledAmount)
    {
        if (Status != SettlementStatus.InProgress)
            throw new InvalidOperationException("Only InProgress settlements can be settled.");
        SettledAmount = settledAmount; Status = SettlementStatus.Settled;
        SettledAt = DateTime.UtcNow; UpdatedAt = DateTime.UtcNow;
    }

    public void Dispute(string reason)
    {
        if (Status == SettlementStatus.Settled || Status == SettlementStatus.Cancelled)
            throw new InvalidOperationException($"Cannot dispute a {Status} settlement.");
        Status = SettlementStatus.Disputed; Notes = reason; UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == SettlementStatus.Settled)
            throw new InvalidOperationException("Cannot cancel a Settled settlement.");
        Status = SettlementStatus.Cancelled; UpdatedAt = DateTime.UtcNow;
    }
}
