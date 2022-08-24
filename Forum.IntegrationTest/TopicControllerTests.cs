using FluentAssertions;
using Forum.Authorization;
using Forum.Entities;
using Forum.IntegrationTest.Helper;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Forum.IntegrationTest
{
    public class TopicControllerTests : TopicTestsHelper
    {

        public TopicControllerTests() : base()
        {

        }
        [Fact]
        public async Task Create_ForValidModel_ReturnsOk()
        {
            //Arrange
            var topic = new TopicDto
            {
                NameOfTopic = "Asdf",
                Description = "Asdfdesc"
            };
            var httpContent = topic.SerializeForHttp();

            //Act
            var response = await _client.PostAsync("api/forum/createTopic", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Create_ForInvalidModel_ReturnsBadRequest()
        {
            //Arrange
            var topic = new TopicDto
            {

                Description = "Asdfdesc"
            };
            var httpContent = topic.SerializeForHttp();

            //Act
            var response = await _client.PostAsync("api/forum/createTopic", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        }
        [Fact]
        public async Task Delete_ForIncorrectId_ReturnsnNotFound()
        {
            //Arrange
            //await SeedUsers();
            int id = 22;
            //Act
            var response = await _client.DeleteAsync($"api/forum/{id}");
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        }
        [Fact]
        public async Task Delete_ForCorrectId_ReturnsNoContent()
        {
            //Arrange
           // await SeedUsers();
            int id = 1;
           // Act
            var response = await _client.DeleteAsync($"api/forum/{id}");
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        }
        [Fact]
        public async Task Update_ForValidModel_ReturnsOk()
        {
            //Arrange
            var model = new UpdateTopicDto
            {
                Description = "GHJKdesctest"
            };
            int id = 1;
            //await SeedUsers();
            var httpContent = model.SerializeForHttp();
            //Act
            var response = await _client.PutAsync($"api/forum/{id}/update", httpContent);
           // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetAll_ForValidPaginationModel_ReturnsOk()
        {
            //Arrange
           // await SeedUsers();
            //Act
            var response = await _client.GetAsync("api/forum/?pageSize=1&pageNumber=1");
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetAll_WithoutPaginationModel_ReturnsNotFound()
        {
            //Arrange
            //await SeedUsers();
            //Act
            var response = await _client.GetAsync("api/forum/");
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task GetById_ForCorrectId_ReturnsOk()
        {
            //Arrange
            int id = 2;
            //await SeedUsers();

            //Act
            var response = await _client.GetAsync($"api/forum/{id}");
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetById_ForIncorrectId_ReturnsNotFound()
        {
            //Arrange
            int id = 20;
            //await SeedUsers();
            //Act
            var response = await _client.GetAsync($"api/forum/{id}");
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }


    }
}
