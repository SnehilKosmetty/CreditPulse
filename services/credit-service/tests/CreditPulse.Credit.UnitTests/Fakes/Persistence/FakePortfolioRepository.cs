using CreditPulse.Credit.Application.Abstractions.Persistence;
using CreditPulse.Credit.Domain.Entities;

namespace CreditPulse.Credit.UnitTests.Fakes.Persistence;

public sealed class FakePortfolioRepository : IPortfolioRepository
{
    public Portfolio? SavedPortfolio { get; private set; }

    public Task AddAsync(
        Portfolio portfolio,
        CancellationToken cancellationToken = default)
    {
        SavedPortfolio = portfolio;

        return Task.CompletedTask;
    }
}