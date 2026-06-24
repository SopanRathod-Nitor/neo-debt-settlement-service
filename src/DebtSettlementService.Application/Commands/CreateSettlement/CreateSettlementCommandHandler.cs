using DebtSettlementService.Application.Common;
using DebtSettlementService.Application.DTOs;
using DebtSettlementService.Application.Interfaces;
using DebtSettlementService.Domain.Entities;
using DebtSettlementService.Domain.Enums;
using DebtSettlementService.Domain.ValueObjects;
using MediatR;

namespace DebtSettlementService.Application.Commands.CreateSettlement;

internal sealed class CreateSettlementCommandHandler(ISettlementRepository repository)
    : IRequestHandler<CreateSettlementCommand, SettlementDto>
{
    public async Task<SettlementDto> Handle(CreateSettlementCommand request, CancellationToken ct)
    {
        if (!Enum.TryParse<PaymentMethod>(request.PaymentMethod, true, out var paymentMethod))
            throw new ArgumentException($"Unknown payment method: {request.PaymentMethod}");

        var settlement = Settlement.Create(
            request.DebtorName, request.DebtorAccountNumber, request.CreditorName,
            new Money(request.OriginalAmount, request.Currency),
            paymentMethod,
            DateOnly.Parse(request.DueDate),
            request.Notes);

        await repository.AddAsync(settlement, ct);
        return SettlementMapper.ToDto(settlement);
    }
}
