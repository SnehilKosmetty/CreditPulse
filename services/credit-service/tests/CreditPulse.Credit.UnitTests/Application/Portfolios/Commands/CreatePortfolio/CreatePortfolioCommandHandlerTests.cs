using CreditPulse.Credit.Application.Portfolios.Commands.CreatePortfolio;
using CreditPulse.Credit.Domain.Entities;
using CreditPulse.Credit.Domain.Enums;
using CreditPulse.Credit.UnitTests.Fakes.Persistence;

namespace CreditPulse.Credit.UnitTests.Application.Portfolios.Commands.CreatePortfolio;

public sealed class CreatePortfolioCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_SavesPortfolio()
    {
        // Arrange
        var repository = new FakePortfolioRepository();

        var handler =
            new CreatePortfolioCommandHandler(repository);

        var command = new CreatePortfolioCommand(
            "rmbs-2026-001",
            "Residential Mortgage Portfolio 2026",
            PortfolioType.ResidentialMortgageBackedSecurities,
            "usd",
            250_000_000m);

        // Act
        Guid portfolioId = await handler.HandleAsync(command);

        // Assert
        Portfolio? savedPortfolio = repository.SavedPortfolio;

        Assert.NotNull(savedPortfolio);
        Assert.Equal(portfolioId, savedPortfolio.Id);
        Assert.Equal("RMBS-2026-001", savedPortfolio.Code);
        Assert.Equal(
            "Residential Mortgage Portfolio 2026",
            savedPortfolio.Name);
        Assert.Equal(
            PortfolioType.ResidentialMortgageBackedSecurities,
            savedPortfolio.Type);
        Assert.Equal("USD", savedPortfolio.Currency);
        Assert.Equal(
            250_000_000m,
            savedPortfolio.OriginalBalance);
        Assert.Equal(
            PortfolioStatus.Active,
            savedPortfolio.Status);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidName_DoesNotSavePortfolio()
    {
        // Arrange
        var repository = new FakePortfolioRepository();

        var handler =
            new CreatePortfolioCommandHandler(repository);

        var command = new CreatePortfolioCommand(
            "RMBS-2026-001",
            "",
            PortfolioType.ResidentialMortgageBackedSecurities,
            "USD",
            250_000_000m);

        // Act
        Func<Task> action = () =>
            handler.HandleAsync(command);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(action);

        Assert.Null(repository.SavedPortfolio);
    }
}