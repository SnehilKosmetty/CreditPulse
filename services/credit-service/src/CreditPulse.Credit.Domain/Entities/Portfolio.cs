using CreditPulse.Credit.Domain.Enums;

namespace CreditPulse.Credit.Domain.Entities;

public sealed class Portfolio
{
    private Portfolio()
    {
    }

    private Portfolio(
        Guid id,
        string code,
        string name,
        PortfolioType type,
        string currency,
        decimal originalBalance,
        DateTimeOffset createdAtUtc)
    {
        Id = id;
        Code = code;
        Name = name;
        Type = type;
        Currency = currency;
        OriginalBalance = originalBalance;
        CurrentBalance = originalBalance;
        Status = PortfolioStatus.Active;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; private set; }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public PortfolioType Type { get; private set; }

    public string Currency { get; private set; } = string.Empty;

    public decimal OriginalBalance { get; private set; }

    public decimal CurrentBalance { get; private set; }

    public PortfolioStatus Status { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset? UpdatedAtUtc { get; private set; }

    public static Portfolio Create(
        string code,
        string name,
        PortfolioType type,
        string currency,
        decimal originalBalance)
    {
        ValidateCode(code);
        ValidateName(name);
        ValidateType(type);
        ValidateCurrency(currency);
        ValidateBalance(originalBalance);

        return new Portfolio(
            Guid.NewGuid(),
            code.Trim().ToUpperInvariant(),
            name.Trim(),
            type,
            currency.Trim().ToUpperInvariant(),
            originalBalance,
            DateTimeOffset.UtcNow);
    }

    public void UpdateDetails(
        string name,
        PortfolioType type,
        string currency)
    {
        ValidateName(name);
        ValidateType(type);
        ValidateCurrency(currency);

        Name = name.Trim();
        Type = type;
        Currency = currency.Trim().ToUpperInvariant();
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void UpdateCurrentBalance(decimal currentBalance)
    {
        if (currentBalance < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(currentBalance),
                "Current balance cannot be negative.");
        }

        if (currentBalance > OriginalBalance)
        {
            throw new ArgumentOutOfRangeException(
                nameof(currentBalance),
                "Current balance cannot be greater than the original balance.");
        }

        CurrentBalance = currentBalance;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void MarkUnderReview()
    {
        Status = PortfolioStatus.UnderReview;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Activate()
    {
        Status = PortfolioStatus.Active;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Close()
    {
        Status = PortfolioStatus.Closed;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    private static void ValidateCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException(
                "Portfolio code is required.",
                nameof(code));
        }

        if (code.Trim().Length > 20)
        {
            throw new ArgumentException(
                "Portfolio code cannot exceed 20 characters.",
                nameof(code));
        }
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Portfolio name is required.",
                nameof(name));
        }

        if (name.Trim().Length > 150)
        {
            throw new ArgumentException(
                "Portfolio name cannot exceed 150 characters.",
                nameof(name));
        }
    }

    private static void ValidateType(PortfolioType type)
    {
        if (!Enum.IsDefined(type))
        {
            throw new ArgumentException(
                "A valid portfolio type is required.",
                nameof(type));
        }
    }

    private static void ValidateCurrency(string currency)
    {
        if (string.IsNullOrWhiteSpace(currency) ||
            currency.Trim().Length != 3)
        {
            throw new ArgumentException(
                "Currency must contain a three-letter code, such as USD.",
                nameof(currency));
        }
    }

    private static void ValidateBalance(decimal originalBalance)
    {
        if (originalBalance <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(originalBalance),
                "Original balance must be greater than zero.");
        }
    }
}