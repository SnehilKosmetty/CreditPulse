using CreditPulse.Credit.Domain.Entities;
using CreditPulse.Credit.Domain.Enums;

namespace CreditPulse.Credit.UnitTests.Entities;

public sealed class PortfolioTests
{
    [Fact]
    public void Create_WithValidInformation_CreatesActivePortfolio()
    {
        // Arrange
        const string code = "rmbs-2026-001";
        const string name = "Residential Mortgage Portfolio 2026";
        const PortfolioType type =
            PortfolioType.ResidentialMortgageBackedSecurities;
        const string currency = "usd";
        const decimal originalBalance = 250_000_000m;

        // Act
        Portfolio portfolio = Portfolio.Create(
            code,
            name,
            type,
            currency,
            originalBalance);

        // Assert
        Assert.NotEqual(Guid.Empty, portfolio.Id);
        Assert.Equal("RMBS-2026-001", portfolio.Code);
        Assert.Equal(name, portfolio.Name);
        Assert.Equal(type, portfolio.Type);
        Assert.Equal("USD", portfolio.Currency);
        Assert.Equal(originalBalance, portfolio.OriginalBalance);
        Assert.Equal(originalBalance, portfolio.CurrentBalance);
        Assert.Equal(PortfolioStatus.Active, portfolio.Status);
        Assert.NotEqual(default, portfolio.CreatedAtUtc);
        Assert.Null(portfolio.UpdatedAtUtc);
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        const string emptyName = "";

        // Act
        Action action = () => Portfolio.Create(
            "RMBS-2026-001",
            emptyName,
            PortfolioType.ResidentialMortgageBackedSecurities,
            "USD",
            250_000_000m);

        // Assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(action);

        Assert.Equal("name", exception.ParamName);
        Assert.Contains("Portfolio name is required", exception.Message);
    }

    [Fact]
    public void Create_WithInvalidCurrency_ThrowsArgumentException()
    {
        // Arrange
        const string invalidCurrency = "US";

        // Act
        Action action = () => Portfolio.Create(
            "RMBS-2026-001",
            "Residential Mortgage Portfolio",
            PortfolioType.ResidentialMortgageBackedSecurities,
            invalidCurrency,
            250_000_000m);

        // Assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(action);

        Assert.Equal("currency", exception.ParamName);
        Assert.Contains(
            "Currency must contain a three-letter code",
            exception.Message);
    }

    [Fact]
    public void Create_WithZeroOriginalBalance_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        const decimal invalidBalance = 0m;

        // Act
        Action action = () => Portfolio.Create(
            "RMBS-2026-001",
            "Residential Mortgage Portfolio",
            PortfolioType.ResidentialMortgageBackedSecurities,
            "USD",
            invalidBalance);

        // Assert
        ArgumentOutOfRangeException exception =
            Assert.Throws<ArgumentOutOfRangeException>(action);

        Assert.Equal("originalBalance", exception.ParamName);
        Assert.Contains(
            "Original balance must be greater than zero",
            exception.Message);
    }

    [Fact]
    public void UpdateCurrentBalance_WithValidBalance_UpdatesBalanceAndTime()
    {
        // Arrange
        Portfolio portfolio = CreateValidPortfolio();
        const decimal newBalance = 200_000_000m;

        // Act
        portfolio.UpdateCurrentBalance(newBalance);

        // Assert
        Assert.Equal(newBalance, portfolio.CurrentBalance);
        Assert.NotNull(portfolio.UpdatedAtUtc);
    }

    [Fact]
    public void UpdateCurrentBalance_WhenGreaterThanOriginalBalance_ThrowsException()
    {
        // Arrange
        Portfolio portfolio = CreateValidPortfolio();
        const decimal invalidBalance = 300_000_000m;

        // Act
        Action action = () =>
            portfolio.UpdateCurrentBalance(invalidBalance);

        // Assert
        ArgumentOutOfRangeException exception =
            Assert.Throws<ArgumentOutOfRangeException>(action);

        Assert.Equal("currentBalance", exception.ParamName);
        Assert.Contains(
            "Current balance cannot be greater than the original balance",
            exception.Message);
    }

    [Fact]
    public void MarkUnderReview_WhenCalled_ChangesStatus()
    {
        // Arrange
        Portfolio portfolio = CreateValidPortfolio();

        // Act
        portfolio.MarkUnderReview();

        // Assert
        Assert.Equal(PortfolioStatus.UnderReview, portfolio.Status);
        Assert.NotNull(portfolio.UpdatedAtUtc);
    }

    [Fact]
    public void Close_WhenCalled_ChangesStatusToClosed()
    {
        // Arrange
        Portfolio portfolio = CreateValidPortfolio();

        // Act
        portfolio.Close();

        // Assert
        Assert.Equal(PortfolioStatus.Closed, portfolio.Status);
        Assert.NotNull(portfolio.UpdatedAtUtc);
    }

    private static Portfolio CreateValidPortfolio()
    {
        return Portfolio.Create(
            "RMBS-2026-001",
            "Residential Mortgage Portfolio 2026",
            PortfolioType.ResidentialMortgageBackedSecurities,
            "USD",
            250_000_000m);
    }
}