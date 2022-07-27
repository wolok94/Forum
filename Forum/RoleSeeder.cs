using Forum.Entities;

namespace Forum
{
    public class RoleSeeder
    {
        private readonly ForumDbContext dbContext;

        public RoleSeeder(ForumDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void seedRoles()
        {
            if (dbContext.Database.CanConnect())
            {
                if (!dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    dbContext.Roles.AddRange(roles);
                    dbContext.SaveChanges();
                }
            }
        }

        public IEnumerable<Role> GetRoles()
        {
            List<Role> roles = new List<Role>()
                {
                    new Role()
                    {
                        Id = 1,
                        Name = "User"
                    },
                    new Role()
                    {
                        Id = 2,
                        Name = "Admin"
                    }
                };
            return roles;
        }
    }

}
