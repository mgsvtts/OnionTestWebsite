using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(nameof(OrderItem));

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .ValueGeneratedOnAdd();

        builder.Property(item => item.Quantity)
            .HasPrecision(18, 3);
    }
}