using DebtSettlementService.Application.DTOs;
using DebtSettlementService.Application.Interfaces;
using DebtSettlementService.Domain.Entities;
using DebtSettlementService.Domain.Enums;
using DebtSettlementService.Domain.ValueObjects;

namespace DebtSettlementService.Application.Services;

public sealed class SettlementService(ISettlementRepository repository)
{
    public async Task<IReadOnlyList<SettlementDto>> GetAllAsync(string? status, CancellationToken ct)
    {
        if (status is not null && Enum.TryParse<SettlementStatus>(status, true, out var parsed))
            return (await repository.GetByStatusAsync(parsed, ct)).Select(ToDto).ToList();
        return (await repository.GetAllAsync(ct)).Select(ToDto).ToList();
    }

    public async Task<SettlementDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var s = await repository.GetByIdAsync(id, ct);
        return s is null ? null : ToDto(s);
    }

    public async Task<SettlementDto> CreateAsync(CreateSettlementRequest req, CancellationToken ct)
    {
        if (!Enum.TryParse<PaymentMethod>(req.PaymentMethod, true, out var pm))
            throw new ArgumentException($"Unknown payment method: {req.PaymentMethod}");
        var s = Settlement.Create(req.DebtorName, req.DebtorAccountNumber, req.CreditorName,
            new Money(req.OriginalAmount, req.Currency), pm, DateOnly.Parse(req.DueDate), req.Notes);
        await repository.AddAsync(s, ct);
        return ToDto(s);
    }

    public async Task<SettlementDto> StartProcessingAsync(Guid id, CancellationToken ct)
    { var s = await GetOrThrowAsync(id, ct); s.StartProcessing(); await repository.UpdateAsync(s, ct); return ToDto(s); }

    public async Task<SettlementDto> MarkSettledAsync(Guid id, SettleRequest req, CancellationToken ct)
    { var s = await GetOrThrowAsync(id, ct); s.MarkSettled(new Money(req.SettledAmount, req.Currency)); await repository.UpdateAsync(s, ct); return ToDto(s); }

    public async Task<SettlementDto> DisputeAsync(Guid id, DisputeRequest req, CancellationToken ct)
    { var s = await GetOrThrowAsync(id, ct); s.Dispute(req.Reason); await repository.UpdateAsync(s, ct); return ToDto(s); }

    public async Task<SettlementDto> CancelAsync(Guid id, CancellationToken ct)
    { var s = await GetOrThrowAsync(id, ct); s.Cancel(); await repository.UpdateAsync(s, ct); return ToDto(s); }

    private async Task<Settlement> GetOrThrowAsync(Guid id, CancellationToken ct)
        => await repository.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException($"Settlement {id} not found.");

    private static SettlementDto ToDto(Settlement s) => new(
        s.Id, s.DebtorName, s.DebtorAccountNumber, s.CreditorName,
        s.OriginalAmount.Amount, s.SettledAmount.Amount, s.OriginalAmount.Currency,
        s.Status.ToString(), s.PaymentMethod.ToString(),
        s.DueDate.ToString("yyyy-MM-dd"), s.SettledAt?.ToString("yyyy-MM-dd HH:mm:ss"),
        s.Notes, s.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
}
