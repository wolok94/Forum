using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Entities
{
    public class ForumDbContext :DbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>(eb =>
            {
                eb.HasOne(u => u.User).WithMany(t => t.Topics).HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.ClientCascade);
                eb.Property(x => x.NameOfTopic).IsRequired();
            });

            modelBuilder.Entity<User>(eb =>
            {
                eb.HasMany(c => c.Comments).WithOne(u => u.User).HasForeignKey(c => c.UserId);
            });


            modelBuilder.Entity<Role>(eb =>
            {
                eb.HasMany(u => u.Users).WithOne(r => r.Role).HasForeignKey(u => u.RoleId);
                eb.Property(x => x.Name).IsRequired();
            });






        }


    }
}
