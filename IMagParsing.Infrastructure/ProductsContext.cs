using IMagParsing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace IMagParsing.Infrastructure;

public sealed class ProductsContext : DbContext
{
    public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
    {
    }

    public DbSet<ProductParsing> ProductParsings => Set<ProductParsing>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductParsing>(entity =>
            {
                entity.ToTable("ProductParsing");

                entity.HasKey(e => e.ProductParsingId);

                entity.Property(e => e.ProductParsingId)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(520);

                entity.Property(e => e.ColorType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StorageSize)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.ParsingDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ActualStatus)
                    .IsRequired()
                    .HasColumnType("integer");
            }
        );

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasKey(e => e.UserId);

            entity.Property(e => e.IsSubscribe)
                .HasDefaultValue(false);
        });

        base.OnModelCreating(modelBuilder);
    }
}