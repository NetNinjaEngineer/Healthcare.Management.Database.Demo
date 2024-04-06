using Healthcare.Management.Database.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healthcare.Management.Database.Demo.Data.Config
{
    internal class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.FirstName)
                .HasColumnType("varchar").HasMaxLength(50).IsRequired();

            builder.Property(x => x.LastName)
                .HasColumnType("varchar").HasMaxLength(50).IsRequired();

            builder.Property(x => x.Gender)
                .HasColumnType("varchar")
                .HasMaxLength(10).IsRequired();

            builder.Property(x => x.Phone)
               .HasColumnType("varchar")
               .HasMaxLength(20).IsRequired();

            builder.Property(x => x.Email)
               .HasColumnType("varchar")
               .HasMaxLength(100).IsRequired();

            builder.ToTable("Patients");
        }
    }
}
