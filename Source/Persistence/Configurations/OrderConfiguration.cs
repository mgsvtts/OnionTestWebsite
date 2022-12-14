using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order));

        builder.HasKey(order => order.Id);

        builder.Property(order => order.Id)
            .ValueGeneratedOnAdd();

        builder.Property(order => order.Date)
            .IsRequired();

        builder.HasIndex(order => order.Number);

        builder.HasMany(order => order.OrderItems)
            .WithOne(item => item.Order)
            .OnDelete(DeleteBehavior.Cascade);
    }
}