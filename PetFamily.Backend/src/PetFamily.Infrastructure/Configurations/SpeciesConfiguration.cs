using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Species;

namespace PetFamily.Infrastructure.Configurations;

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
            .HasMaxLength(Constants.MaxLowTextLength);

        builder.OwnsOne(
            s => s.Breeds,
            bb =>
            {
                bb.ToJson("species_breeds");
                bb.OwnsMany(
                    sbb => sbb.Values,
                    b =>
                    {
                        b.Property(bp => bp.Name)
                            .IsRequired()
                            .HasMaxLength(Constants.MaxLowTextLength)
                            .HasColumnName("breed_name");

                        b.Property(br => br.Id)
                            .IsRequired()
                            .HasConversion(
                                id => id.Value,
                                value => BreedId.Create(value));
                    });
            });
    }
}