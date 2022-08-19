using FluentAssertions;
using Forum.Entities;
using Forum.Exceptions;
using Forum.IntegrationTest.Helper;
using Forum.Models;
using Forum.Pagination;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Forum.IntegrationTest
{
    public class CommentControllerTests 
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;



        public CommentControllerTests() 
        {
            _factory = new WebApplicationFactory<Program>()
    .WithWebHostBuilder(builder =>
    {
        builder.ConfigureServices(services =>
        {
            var dbContext = services.SingleOrDefault(c => c.ServiceType == typeof(DbContextOptions<ForumDbContext>));
            services.Remove(dbContext);

            services.AddDbContext<ForumDbContext>(options => options.UseInMemoryDatabase("ForumDb"));
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
        public List<User> GetUser()
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
        [Fact]
        public async Task Delete_ForCorrectId_ReturnsOk()
        {
            //Arrange

            var id = 1;
            await SeedUsers();
            //Act
            var response = await _client.DeleteAsync($"api/forum/{id}/comments");
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Delete_ForIncorrectId_ReturnsNotFound()
        {
            //Arrange
            
            int id = 10;
            //Act
            var response = await _client.DeleteAsync($"api/forum/{id}/comments");
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Create_ForValidModel_ReturnsOk()
        {
            //Arrange
            var model = new CommentDto()
            {
                Description = "AsdDesc"
            };
            var id = 1;

            //Act
            var httpContent = model.SerializeForHttp();
            var response = await _client.PostAsync($"api/forum/{id}/comments", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Create_ForInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            var model = new CommentDto()
            {

            };
            var id = 1;

            //Act
            var httpContent = model.SerializeForHttp();
            var response = await _client.PostAsync($"api/forum/{id}/comments", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task GetAll_ForCorrectPaginationModel_ReturnsOk()
        {
            //Arrange
            await SeedUsers();
            //Act
            var response = await _client.GetAsync($"api/forum/1/comments/?pageSize=1&pageNumber=1");
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetAll_ForIncorrectId_ReturnsNotFounded()
        {
            //Arrange

            //Act
            var response = await _client.GetAsync($"api/forum/10/comments/?pageSize=1&pageNumber=1");
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Update_ForCorrectId_ReturnsOk()
        {
            //Arrange
            int commentId = 1;
            string description = "hahahaha";
            await SeedUsers();
            var httpContent = description.SerializeForHttp();
            //Act
            var response = await _client.PutAsync($"api/forum/comments/{commentId}", httpContent);
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


    }
}
