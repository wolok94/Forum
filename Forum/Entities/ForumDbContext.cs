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
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }


    }
}
