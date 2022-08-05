using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Entities.Configurations
{
    public class TopicConfigurations : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> eb)
        {
            eb.HasOne(u => u.User).WithMany(t => t.Topics).HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.ClientCascade);
            eb.Property(x => x.NameOfTopic).IsRequired();
        }
    }
}
