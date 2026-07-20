using CreditPulse.Credit.Application.Abstractions.Persistence;
using CreditPulse.Credit.Domain.Entities;

namespace CreditPulse.Credit.Application.Portfolios.Commands.CreatePortfolio;

public sealed class CreatePortfolioCommandHandler
{
    private readonly IPortfolioRepository _portfolioRepository;

    public CreatePortfolioCommandHandler(
        IPortfolioRepository portfolioRepository)
    {
        _portfolioRepository = portfolioRepository;
    }

    public async Task<Guid> HandleAsync(
        CreatePortfolioCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        Portfolio portfolio = Portfolio.Create(
            command.Code,
            command.Name,
            command.Type,
            command.Currency,
            command.OriginalBalance);

        await _portfolioRepository.AddAsync(
            portfolio,
            cancellationToken);

        return portfolio.Id;
    }
}