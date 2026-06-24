namespace DebtSettlementService.Domain.ValueObjects;

public sealed record Money(decimal Amount, string Currency = "INR")
{
    public static Money Zero => new(0);

    public Money Add(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException($"Cannot add {other.Currency} to {Currency}.");
        return this with { Amount = Amount + other.Amount };
    }
}
