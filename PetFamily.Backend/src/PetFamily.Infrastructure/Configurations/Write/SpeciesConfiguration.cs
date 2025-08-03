using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.SpeciesManagement;

namespace PetFamily.Infrastructure.Configurations.Write;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value));

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(Domain.Shared.Constants.MaxLowTextLength);

        builder.OwnsMany(
            s => s.Breeds,
            bb =>
            {
                bb.ToJson("species_breeds");

                bb.Property(bp => bp.Name)
                    .IsRequired()
                    .HasMaxLength(Domain.Shared.Constants.MaxLowTextLength)
                    .HasColumnName("breed_name");

                bb.Property(br => br.Id)
                    .IsRequired()
                    .HasConversion(
                        id => id.Value,
                        value => BreedId.Create(value));
            });
    }
}