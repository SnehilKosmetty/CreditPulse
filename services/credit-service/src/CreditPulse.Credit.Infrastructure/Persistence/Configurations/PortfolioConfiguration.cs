using CreditPulse.Credit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditPulse.Credit.Infrastructure.Persistence.Configurations;

public sealed class PortfolioConfiguration
    : IEntityTypeConfiguration<Portfolio>
{
    public void Configure(
        EntityTypeBuilder<Portfolio> builder)
    {
        builder.ToTable("Portfolios", "credit");

        builder.HasKey(portfolio => portfolio.Id);

        builder.Property(portfolio => portfolio.Code)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(portfolio => portfolio.Code)
            .IsUnique();

        builder.Property(portfolio => portfolio.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(portfolio => portfolio.Type)
            .HasConversion<string>()
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(portfolio => portfolio.Currency)
            .HasMaxLength(3)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(portfolio => portfolio.OriginalBalance)
            .HasPrecision(19, 4)
            .IsRequired();

        builder.Property(portfolio => portfolio.CurrentBalance)
            .HasPrecision(19, 4)
            .IsRequired();

        builder.Property(portfolio => portfolio.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(portfolio => portfolio.CreatedAtUtc)
            .IsRequired();

        builder.Property(portfolio => portfolio.UpdatedAtUtc);
    }
}