namespace Ordin.Domain.ValueObjects;

public class Money
{
    private const string ValueCannotBeNegative = "Value cannot be negative";
    public decimal Value { get; private set; }

    private Money(decimal value)
    {
        Value = value;
    }

    /// <summary>
    /// For EF Core mapping use only
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Money ByPass(decimal value)
    {
        return new Money(value);
    }

    //TODO: Change exception to ErrorOr
    public static Money Create(decimal value)
    {
        var error = Validate(value);

        return !string.IsNullOrWhiteSpace(error) ? throw new ArgumentException(error) : new Money(value);
    }

    private static string Validate(decimal value)
    {
        return value < 0 ? ValueCannotBeNegative : string.Empty;
    }

    public static implicit operator Money(decimal value)
    {
        return Create(value);
    }

    public static explicit operator decimal(Money money)
    {
        return money.Value;
    }
}