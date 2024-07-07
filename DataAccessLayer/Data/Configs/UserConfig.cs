using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(u => u.UserName)
                   .IsUnique();

            builder.Property(u => u.HashedPassword)
                   .IsRequired();

            builder.HasOne(u => u.Role)
                   .WithMany(r => r.Users)
                   .IsRequired();

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserId);
        }
    }
}
