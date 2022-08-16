using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Forum.IntegrationsTests
{
    public class AccountControllerTests
    {
        private readonly HttpClient _client;

        public AccountControllerTests()
        {
            var appFactory = new WebApplicationFactory<Program>();
            _client = appFactory.CreateClient();
        }
        [Fact]
        public async Task GetById_ForValidId_ReturnsOk()
        {
            var accountId = 8;
            var response = await _client.GetAsync($"/api/account/{accountId}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
