using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Entities.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> eb)
        {
            eb.HasMany(u => u.Users).WithOne(r => r.Role).HasForeignKey(u => u.RoleId);
            eb.Property(x => x.Name).IsRequired();
        }
    }
}
