using CreditPulse.Credit.Application.Abstractions.Persistence;
using CreditPulse.Credit.Domain.Entities;

namespace CreditPulse.Credit.Infrastructure.Persistence.Repositories;

public sealed class PortfolioRepository : IPortfolioRepository
{
    private readonly CreditDbContext _dbContext;

    public PortfolioRepository(
        CreditDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        Portfolio portfolio,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Portfolios.AddAsync(
            portfolio,
            cancellationToken);

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}