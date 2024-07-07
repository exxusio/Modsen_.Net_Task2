using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configs
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(r => r.Users)
                   .WithOne(u => u.Role)
                   .HasForeignKey(u => u.RoleId);
        }
    }
}
