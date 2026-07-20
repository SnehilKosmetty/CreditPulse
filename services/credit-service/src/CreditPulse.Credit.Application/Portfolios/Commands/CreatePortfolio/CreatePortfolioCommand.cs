using CreditPulse.Credit.Domain.Enums;

namespace CreditPulse.Credit.Application.Portfolios.Commands.CreatePortfolio;

public sealed record CreatePortfolioCommand(
    string Code,
    string Name,
    PortfolioType Type,
    string Currency,
    decimal OriginalBalance);