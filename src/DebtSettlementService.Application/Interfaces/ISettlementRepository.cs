using DebtSettlementService.Domain.Entities;
using DebtSettlementService.Domain.Enums;

namespace DebtSettlementService.Application.Interfaces;

public interface ISettlementRepository
{
    Task<IReadOnlyList<Settlement>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Settlement>> GetByStatusAsync(SettlementStatus status, CancellationToken ct = default);
    Task<Settlement?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Settlement settlement, CancellationToken ct = default);
    Task UpdateAsync(Settlement settlement, CancellationToken ct = default);
}
