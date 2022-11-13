using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal sealed class ProviderConfiguration : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.ToTable(nameof(Provider));

            builder.HasKey(provider => provider.Id);

            builder.Property(provider => provider.Id).ValueGeneratedOnAdd();

            builder.HasIndex(provider => provider.Name);
        }
    }
}