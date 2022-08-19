using Forum.Entities;
using Forum.IntegrationTest.Helper;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Forum.IntegrationTest
{
    public class AccountTestsHelper
    {
        protected HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        private IPasswordHasher<User> _passwordHasher; 


        protected AccountTestsHelper()
        {
                    _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContext = services.SingleOrDefault(c => c.ServiceType == typeof(DbContextOptions<ForumDbContext>));
                    services.Remove(dbContext);

                    services.AddDbContext<ForumDbContext>(options => options.UseInMemoryDatabase("Test"));
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));


                });
            });
                    _client = _factory.CreateClient();
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
            _passwordHasher = new PasswordHasher<User>();
            var users = new List<User>
            {

                new User
                {

                    Nick = "Test2",
                    Role = new Role
                    {
                        Name = "Admin"
                    },
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
            User user = new User
            {
                FirstName = "Asdf",
                LastName = "dfgh",
                Email = "Test@test.com",
                DateOfBirth = DateTime.Now,
                Nick = "Test123",
                Role = new Role
                {
                    Name = "Admin"
                },
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
            };
            var hasherPassword = _passwordHasher.HashPassword(user, "test1");
            user.PasswordHash = hasherPassword;
            users.Add(user);
            return users;
        }
    }
}
