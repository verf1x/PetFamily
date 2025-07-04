using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Volunteers.Entities;
using PetFamily.Domain.Volunteers.Enums;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                p => p.Value,
                value => PetId.Create(value));

        builder.ComplexProperty(p => p.Nickname,
            nb =>
            {
                nb.Property(n => n.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MaxLowTextLength)
                    .HasColumnName("nickname");
            });

        builder.ComplexProperty(p => p.Description,
            db =>
            {
                db.Property(d => d.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MaxLongTextLength)
                    .HasColumnName("description");
            });

        builder.ComplexProperty(p => p.SerialNumber,
            snb =>
            {
                snb.Property(sn => sn.Value)
                    .IsRequired()
                    .HasColumnName("serial_number");
            });

        builder.ComplexProperty(p => p.SpeciesBreed,
            bsb =>
            {
                bsb.Property(p => p.BreedId)
                    .IsRequired()
                    .HasColumnName("breed_id")
                    .HasConversion(id => id.Value, value => BreedId.Create(value));

                bsb.Property(p => p.SpeciesId)
                    .IsRequired()
                    .HasColumnName("species_id")
                    .HasConversion(id => id.Value, value => SpeciesId.Create(value));
            });

        builder.ComplexProperty(p => p.Color,
            cb =>
            {
                cb.Property(c => c.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MaxLowTextLength)
                    .HasColumnName("color");
            });

        builder.ComplexProperty(p => p.HealthInfo,
            hb =>
            {
                hb.Property(h => h.HealthStatus)
                    .IsRequired()
                    .HasMaxLength(Constants.MaxMediumTextLength)
                    .HasColumnName("health_status");

                hb.Property(h => h.IsNeutered)
                    .IsRequired()
                    .HasColumnName("is_neutered");

                hb.Property(h => h.IsVaccinated)
                    .IsRequired()
                    .HasColumnName("is_vaccinated");
            });

        builder.ComplexProperty(p => p.Address,
            pab =>
            {
                pab.Property(p => p.Locality)
                    .IsRequired()
                    .HasMaxLength(Constants.MaxLowTextLength);

                pab.Property(p => p.CountryCode)
                    .IsRequired()
                    .HasMaxLength(Constants.MaxCountryCodeLength);

                pab.Property(p => p.Region)
                    .IsRequired(false)
                    .HasMaxLength(Constants.MaxLowTextLength);

                pab.Property(p => p.PostalCode)
                    .IsRequired(false)
                    .HasMaxLength(Constants.MaxLowTextLength);

                pab.Property(p => p.AddressLines)
                    .IsRequired()
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null)!)
                    .HasColumnType("jsonb")
                    .HasColumnName("address_lines");
            });

        builder.ComplexProperty(p => p.Measurements,
            mb =>
            {
                mb.Property(m => m.Height)
                    .IsRequired()
                    .HasColumnName("height")
                    .HasDefaultValue(0);

                mb.Property(m => m.Weight)
                    .IsRequired()
                    .HasColumnName("weight")
                    .HasDefaultValue(0);
            });

        builder.ComplexProperty(p => p.OwnerPhoneNumber,
            opn =>
            {
                opn.Property(p => p.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MaxPhoneNumberLength)
                    .HasColumnName("owner_phone_number");
            });

        builder.Property(p => p.DateOfBirth)
            .IsRequired();

        builder.Property(p => p.HelpStatus)
            .IsRequired()
            .HasDefaultValue(HelpStatus.NeedsHelp);

        builder.OwnsOne(p => p.HelpRequisites,
            hdb =>
            {
                hdb.ToJson("pet_help_requisites");
                hdb.OwnsMany(hd => hd.Values,
                    db =>
                    {
                        db.Property(d => d.Name)
                            .IsRequired()
                            .HasMaxLength(Constants.MaxLowTextLength)
                            .HasColumnName("help_requisite_name");

                        db.Property(d => d.Description)
                            .IsRequired()
                            .HasMaxLength(Constants.MaxMediumTextLength)
                            .HasColumnName("help_requisite_description");
                    });
            });

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property<bool>("IsDeleted")
            .HasColumnName("is_deleted");

        builder.Property<DateTime?>("DeletionDate")
            .HasColumnName("deletion_date");
    }
}