using FluentAssertions;
using Forum.Entities;
using Forum.IntegrationTests.Helper;
using Forum.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Forum.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AccountControllerTests()
        {
            _client = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContext = services.SingleOrDefault(c => c.ServiceType == typeof(DbContextOptions<ForumDbContext>));
                        services.Remove(dbContext);

                        services.AddDbContext<ForumDbContext>(options => options.UseInMemoryDatabase("ForumDb"));


                    });
                }).CreateClient();
        }

        [Fact]
        public async Task Create_ForValidModel_ReturnsOk()
        {
            //arrange
            var user = new CreateUserDto()
            {
                Nick = "Test",
                Email = "test@test.com",
                Password = "test1"
            };

            //act
            var httpContent = user.SerializeForHttp();
            var response = await _client.PostAsync("api/account/register", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Create_ForInvalidModel_ReturnsBadRequest()
        {
            //arrange
            var user = new CreateUserDto()
            {
                
                Email = "test@test.com",
                Password = "test1"
            };

            //act
            var httpContent = user.SerializeForHttp();
            var response = await _client.PostAsync("api/account/register", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
