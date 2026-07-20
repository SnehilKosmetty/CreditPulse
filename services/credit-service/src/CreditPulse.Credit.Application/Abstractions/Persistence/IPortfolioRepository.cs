using CreditPulse.Credit.Domain.Entities;

namespace CreditPulse.Credit.Application.Abstractions.Persistence;

public interface IPortfolioRepository
{
    Task AddAsync(
        Portfolio portfolio,
        CancellationToken cancellationToken = default);
}