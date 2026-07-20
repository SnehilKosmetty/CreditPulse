using CreditPulse.Credit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditPulse.Credit.Infrastructure.Persistence;

public sealed class CreditDbContext : DbContext
{
    public CreditDbContext(
        DbContextOptions<CreditDbContext> options)
        : base(options)
    {
    }

    public DbSet<Portfolio> Portfolios => Set<Portfolio>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CreditDbContext).Assembly);
    }
}