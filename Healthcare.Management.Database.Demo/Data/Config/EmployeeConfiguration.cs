using Healthcare.Management.Database.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healthcare.Management.Database.Demo.Data.Config
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
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

            builder.Property(x => x.JobTitle)
               .HasColumnType("varchar")
               .HasMaxLength(20).IsRequired();

            builder.Property(x => x.HireDate)
               .HasColumnType("datetime2").IsRequired();

            builder.Property(x => x.Salary)
             .HasColumnType("decimal(18, 2)").IsRequired();

            builder.Property(x => x.DateOfBirth)
              .HasColumnType("datetime2").IsRequired();

            builder.HasOne(x => x.Department)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.DepartmentId).IsRequired();

            builder.ToTable("Employees");
        }
    }
}
