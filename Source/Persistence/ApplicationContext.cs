using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Order> Order { get; set; }

    public DbSet<OrderItem> OrderItem { get; set; }

    public DbSet<Provider> Provider { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
}