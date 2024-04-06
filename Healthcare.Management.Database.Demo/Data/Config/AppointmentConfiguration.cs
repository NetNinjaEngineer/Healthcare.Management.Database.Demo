using Healthcare.Management.Database.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healthcare.Management.Database.Demo.Data.Config
{
    internal class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.AppointmentTime)
               .HasColumnType("time").IsRequired();

            builder.Property(x => x.AppointmentDate)
              .HasColumnType("date").IsRequired();

            builder.HasOne(x => x.Employee)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.EmployeeId).IsRequired();


            builder.HasOne(x => x.Patient)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.PatientId).IsRequired();


            builder.ToTable("Appointments");
        }
    }
}
