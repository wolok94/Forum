using Forum.Entities;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Forum.IntegrationTest.Helper
{
    public class TopicTestsHelper
    {
        private WebApplicationFactory<Program> _factory;
        protected HttpClient _client;
        private string databaseName = new Guid().ToString();
        protected TopicTestsHelper()
        {
            
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                     {

                         
                         builder.ConfigureServices(services =>
                              {
                                  var dbContext = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ForumDbContext>));
                                  services.Remove(dbContext);

                                  services.AddDbContext<ForumDbContext>(options => options.UseInMemoryDatabase(databaseName));
                                  services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                                  services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));
                                  services.AddScoped<RoleSeeder>();
                                  var sp = services.BuildServiceProvider();

                                  using(var scope = sp.CreateScope())
                                  {
                                      var scopedServices = scope.ServiceProvider;
                                      var db = scopedServices.GetRequiredService<ForumDbContext>();

                                      db.Database.EnsureDeleted();
                                      db.Database.EnsureCreated();

                                      try
                                      {
                                          Seeder.SeedUsers(db);
                                      }
                                      catch (Exception ex)
                                      {

                                      }
                                  }

                              });

                              });
            

            _client = _factory.CreateClient();
        }

        //protected async Task SeedUsers()
        //{
        //    var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        //    using var scope = scopeFactory.CreateScope();
        //    var dbContext = scope.ServiceProvider.GetService<ForumDbContext>();
        //    if (!await dbContext.Users.AnyAsync())
        //    {
        //        await dbContext.Users.AddRangeAsync(GetUser());
        //        await dbContext.SaveChangesAsync();
        //    }

        //}
        //public List<User> GetUser()
        //{
        //    var users = new List<User>
        //    {
        //        new User
        //        {
        //            Nick = "Test",
        //            Topics = new List<Topic>
        //            {
        //                new Topic
        //                {
        //                    NameOfTopic = "Asd",
        //                    Id =1,
        //                    UserId = 1,
        //                    Comments = new List<Comment>
        //                    {
        //                        new Comment{
        //                        User = new User(),
        //                        Description = "Hahahaha",
        //                        Id =1,
        //                        TopicId = 1
        //                        },
        //                        new Comment{
        //                        User = new User(),
        //                        Description = "asdf123",
        //                        Id = 2,
        //                        TopicId = 1
        //                         }
        //                    }
        //                }
        //            }
        //        },
        //        new User
        //        {
        //            Nick = "Test2",
        //            Topics = new List<Topic>
        //            {
        //                new Topic
        //                {
        //                    NameOfTopic = "SDF",
        //                    Id =2,
        //                    Comments = new List<Comment>
        //            {
        //                    new Comment{
        //                        User = new User(),
        //                        Description = "Hahahaha",
        //                        Id =3,
        //                        TopicId = 2
        //                        },
        //                        new Comment{
        //                        User = new User(),
        //                        Description = "asdf123",
        //                        Id = 4,
        //                        TopicId=2
        //                        }
        //            }

        //                }
        //            }

        //        }
        //    };
        //    return users;
        //}
    }
}
