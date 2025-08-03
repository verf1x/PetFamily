using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteersManagement.Entities;

namespace PetFamily.Infrastructure.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value));

        builder.ComplexProperty(v => v.FullName, b =>
        {
            b.Property(fn => fn.FirstName)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MaxLowTextLength)
                .HasColumnName("first_name");
            b.Property(fn => fn.LastName)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MaxLowTextLength)
                .HasColumnName("last_name");
            b.Property(fn => fn.MiddleName)
                .IsRequired(false)
                .HasMaxLength(Domain.Shared.Constants.MaxLowTextLength)
                .HasColumnName("middle_name");
        });

        builder.ComplexProperty(
            v => v.Email,
            veb =>
            {
                veb.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(Domain.Shared.Constants.MaxEmailLength)
                    .HasColumnName("email");
            });

        builder.ComplexProperty(
            v => v.Description,
            vdb =>
            {
                vdb.Property(d => d.Value)
                    .IsRequired()
                    .HasMaxLength(Domain.Shared.Constants.MaxLongTextLength)
                    .HasColumnName("description");
            });

        builder.ComplexProperty(
            v => v.Experience,
            veb =>
            {
                veb.Property(e => e.TotalYears)
                    .IsRequired()
                    .HasColumnName("total_years")
                    .HasDefaultValue(0);
            });

        builder.ComplexProperty(
            v => v.PhoneNumber,
            b =>
            {
                b.Property(pn => pn.Value)
                    .IsRequired()
                    .HasMaxLength(Domain.Shared.Constants.MaxPhoneNumberLength)
                    .HasColumnName("phone_number");
            });

        builder.OwnsMany(
            v => v.SocialNetworks,
            b =>
            {
                b.ToJson("social_networks");
                    b.Property(snn => snn.Name)
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(Domain.Shared.Constants.MaxLowTextLength);

                    b.Property(snn => snn.Url)
                        .IsRequired()
                        .HasColumnName("url")
                        .HasMaxLength(Domain.Shared.Constants.MaxUrlLength);
            });

        builder.OwnsMany(
            v => v.HelpRequisites,
            hdb =>
            {
                hdb.ToJson("help_requisites");
                hdb.Property(d => d.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(Domain.Shared.Constants.MaxLowTextLength);

                hdb.Property(d => d.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(Domain.Shared.Constants.MaxLowTextLength);
            });

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Navigation(v => v.Pets)
            .AutoInclude();

        builder.Property<bool>("IsDeleted")
            .HasColumnName("is_deleted");

        builder.Property<DateTime?>("DeletionDate")
            .HasColumnName("deletion_date");
    }
}