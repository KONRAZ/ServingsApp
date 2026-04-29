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

    /// <summary>
    /// Набор данных для заказов.
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Набор данных для элементов заказа.
    /// </summary>
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация MenuItem
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Article).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.HasIndex(e => e.Article).IsUnique();
        });

        // Конфигурация Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        // Конфигурация OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Article).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Quantity).IsRequired();

            // Связь с Order
            entity.HasOne<Order>()
                .WithMany(o => o.OrderItems)
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с MenuItem через Article
            entity.HasOne<MenuItem>()
                .WithMany()
                .HasForeignKey(e => e.Article)
                .HasPrincipalKey(e => e.Article)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
