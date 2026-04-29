using Microsoft.EntityFrameworkCore;
using Servings.Domain.Entities;

namespace Servings.Infrastructure.Data;

/// <summary>
/// Контекст базы данных для приложения.
/// </summary>
public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    /// <summary>
    /// Набор данных для блюд меню.
    /// </summary>
    public DbSet<MenuItem> MenuItems { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация MenuItem
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ExternalId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Article).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.HasIndex(e => e.ExternalId).IsUnique();
            entity.HasIndex(e => e.Article).IsUnique();
        });
    }
}
