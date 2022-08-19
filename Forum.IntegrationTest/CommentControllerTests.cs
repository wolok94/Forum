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
    public class CommentControllerTests : CommentTestsHelper
    {


        public CommentControllerTests() : base()
        {

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
