using Forum.Entities;
using Forum.IntegrationTest.Helper;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Forum.IntegrationTest
{
    public class Tests
    {
        protected HttpClient _client;
        private WebApplicationFactory<Program> _factory;


        protected Tests()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContext = services.SingleOrDefault(c => c.ServiceType == typeof(DbContextOptions<ForumDbContext>));
                        services.Remove(dbContext);

                        services.AddDbContext<ForumDbContext>(options => options.UseInMemoryDatabase("ForumDbb"));
                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));


                    });
                });
            _client = _factory.CreateClient();
        }

        protected async Task SeedTopicsAndComments()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ForumDbContext>();
            if (!dbContext.Topics.Any())
            {
                await dbContext.Users.AddRangeAsync(GetUser());
            }
            //if (!dbContext.Comments.Any())
            //{
            //    await dbContext.Comments.AddRangeAsync(GetComments());
            //}

            await dbContext.SaveChangesAsync();
        }
        protected async Task SeedUsers()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ForumDbContext>();
            if (!dbContext.Users.Any())
            {
                await dbContext.Users.AddRangeAsync(GetUser());
            }
            await dbContext.SaveChangesAsync();
        }
        protected List<User> GetUser()
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
                            Comments = new List<Comment>
                            {
                                new Comment{

                                Description = "Hahahaha",
                                Id =1,
                                TopicId = 1
                                },
                                new Comment{

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

                                Description = "Hahahaha",
                                Id =3,
                                TopicId = 2
                                },
                                new Comment{

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
        protected List<User> GetUsers()
        {
            var users = new List<User>
            {
                new User
                {
                Id = 5,
                Nick = "Test123",
                Comments = new List<Comment>()
                },
                new User
                {
                Id=6,
                Nick = "Test1233",
                Comments = new List<Comment>()
                },
                new User
                {
                Id=7,
                Nick = "Test1233",
                Comments = new List<Comment>()
                }
            };
            return users;

        }
        protected List<Topic> GetTopics()
        {


            List<Topic> topics = new List<Topic>{
                new Topic
                {
                    NameOfTopic = "ASD",
                    Id = 1,
                    Comments = new List<Comment>()
                },
                new Topic
                    {
                    NameOfTopic = "SDF",
                        Id = 2,
                        Comments = new List<Comment>{
}
                    }
            };
            return topics;


        }
        protected List<Comment> GetComments()
        {

            var comments = new List<Comment>
        {
            new Comment{
                User = new User
                {
                    Nick = "Test"
                },
            Description = "Hahahaha",
            Id =1,
            TopicId = 1
            },
            new Comment{
                    User = new User
                {
                    Nick = "Test2"
                },
            Description = "asdf123",
            Id = 2,
            TopicId = 1
             },
            new Comment{
                    User = new User
                {
                    Nick = "Test3"
                },
            Description = "Hahahaha",
            Id =3,
            TopicId = 2
            },
            new Comment{
                    User = new User
                {
                    Nick = "Test4"
                },
            Description = "asdf123",
            Id = 4,
            TopicId=2
            }
    };
            return comments;

        }
    }
}
