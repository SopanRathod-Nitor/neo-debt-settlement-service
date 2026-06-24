using DebtSettlementService.Application.Interfaces;
using DebtSettlementService.Domain.Entities;
using DebtSettlementService.Domain.Enums;
using DebtSettlementService.Domain.ValueObjects;

namespace DebtSettlementService.Persistence.Repositories;

public sealed class InMemorySettlementRepository : ISettlementRepository
{
    private readonly List<Settlement> _store = Seed();

    public Task<IReadOnlyList<Settlement>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Settlement>>(_store.ToList());

    public Task<IReadOnlyList<Settlement>> GetByStatusAsync(SettlementStatus status, CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Settlement>>(_store.Where(s => s.Status == status).ToList());

    public Task<Settlement?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_store.FirstOrDefault(s => s.Id == id));

    public Task AddAsync(Settlement settlement, CancellationToken ct = default)
    { _store.Add(settlement); return Task.CompletedTask; }

    public Task UpdateAsync(Settlement settlement, CancellationToken ct = default)
        => Task.CompletedTask;

    private static List<Settlement> Seed()
    {
        var s1 = Settlement.Create("Rajesh Kumar",  "ACC-001", "HDFC Bank",  new Money(150000), PaymentMethod.BankTransfer,  DateOnly.FromDateTime(DateTime.Today.AddDays(30)));
        var s2 = Settlement.Create("Priya Sharma",  "ACC-002", "ICICI Bank", new Money(85000),  PaymentMethod.OnlinePayment, DateOnly.FromDateTime(DateTime.Today.AddDays(15)));
        var s3 = Settlement.Create("Amit Patel",    "ACC-003", "SBI",        new Money(220000), PaymentMethod.Cheque,        DateOnly.FromDateTime(DateTime.Today.AddDays(-5)));
        var s4 = Settlement.Create("Sunita Mehta",  "ACC-004", "Axis Bank",  new Money(45000),  PaymentMethod.BankTransfer,  DateOnly.FromDateTime(DateTime.Today.AddDays(60)));
        var s5 = Settlement.Create("Vikram Singh",  "ACC-005", "Kotak Bank", new Money(310000), PaymentMethod.BankTransfer,  DateOnly.FromDateTime(DateTime.Today.AddDays(-10)));
        s2.StartProcessing();
        s3.StartProcessing(); s3.MarkSettled(new Money(210000));
        s5.StartProcessing(); s5.Dispute("Payment reference mismatch.");
        return [s1, s2, s3, s4, s5];
    }
}
