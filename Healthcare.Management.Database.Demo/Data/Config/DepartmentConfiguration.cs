using Healthcare.Management.Database.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Healthcare.Management.Database.Demo.Data.Config
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasColumnType("varchar").HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CheckCost)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.ToTable("Departments");
        }
    }
}
