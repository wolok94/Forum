using FluentAssertions;
using Forum.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Forum.IntegrationTests
{
    public class TopicTests 
    {
        private readonly HttpClient _client;

        public TopicTests()
        {
            _client = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContext = services.SingleOrDefault(c => c.ServiceType == typeof(DbContextOptions<ForumDbContext>));
                        services.Remove(dbContext);

                        services.AddDbContext<ForumDbContext>(options => options.UseInMemoryDatabase("Forum"));
                    });
                }).CreateClient();
        }
        [Theory]
        [InlineData("pageNumber=1&pageSize=1")]
        public async Task GetAll_ReturnsOk(string query)
        {
            //act
            var response = await _client.GetAsync("api/forum/?");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
