using Forum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.IntegrationTest.Helper
{
    public class Seeder
    {
        

        public Seeder()
        {
            
        }

        public static void SeedUsers(ForumDbContext dbContext)
        {

            if (! dbContext.Users.Any())
            {
                var users = GetUser();
                dbContext.Users.AddRange(users);
                dbContext.SaveChanges();
            }

        }
        public static List<User> GetUser()
        {
            var users = new List<User>
            {
                new User
                {
                    Nick = "Test",
                    Topics = new List<Topic>
                    {
                        new Topic
                        {
                            NameOfTopic = "Asd",
                            Id =1,
                            UserId = 1,
                            Comments = new List<Comment>
                            {
                                new Comment{
                                User = new User(),
                                Description = "Hahahaha",
                                Id =1,
                                TopicId = 1
                                },
                                new Comment{
                                User = new User(),
                                Description = "asdf123",
                                Id = 2,
                                TopicId = 1
                                 }
                            }
                        }
                    }
                },
                new User
                {
                    Nick = "Test2",
                    Topics = new List<Topic>
                    {
                        new Topic
                        {
                            NameOfTopic = "SDF",
                            Id =2,
                            Comments = new List<Comment>
                    {
                            new Comment{
                                User = new User(),
                                Description = "Hahahaha",
                                Id =3,
                                TopicId = 2
                                },
                                new Comment{
                                User = new User(),
                                Description = "asdf123",
                                Id = 4,
                                TopicId=2
                                }
                    }

                        }
                    }

                }
            };
            return users;
        }
    }
}
