using FluentAssertions;
using Forum.Entities;
using Forum.IntegrationTests.Helper;
using Forum.Models;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
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
        private readonly Mock<IAccountService> _mock = new Mock<IAccountService>();
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
        [Fact]
        public async Task Login_ForValidModel_ReturnsOk()
        {
            //arrange
             _mock.Setup(x => x.GenerateJWT(It.IsAny<LoginDto>()))
                .ReturnsAsync("jwt");
            var model = new LoginDto()
            {
                Nick = "Test",
                Password = "test1"
            };
            //act
            var httpContent = model.SerializeForHttp();
            var response = await _client.PostAsync("api/account/login", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Login_ForInvalidModel_ReturnsBadRequest()
        {
            //arrange
            _mock.Setup(x => x.GenerateJWT(It.IsAny<LoginDto>()))
               .ReturnsAsync("jwt");
            var model = new LoginDto()
            {
                Nick = "Test"
                
            };
            //act
            var httpContent = model.SerializeForHttp();
            var response = await _client.PostAsync("api/account/login", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task GetAll_ForRegisteredAdmin_ReturnsOk()
        {
            var response = await _client.GetAsync("api/accounts");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }

    }
}
