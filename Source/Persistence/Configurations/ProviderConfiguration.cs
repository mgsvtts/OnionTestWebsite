using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.ToTable(nameof(Provider));

        builder.HasData
        (
            new Provider { Id = 1, Name = "Provider 1" },
            new Provider { Id = 2, Name = "Provider 2" },
            new Provider { Id = 3, Name = "Provider 3" }
        );

        builder.HasKey(provider => provider.Id);

        builder.Property(provider => provider.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(provider => provider.Name);
    }
}