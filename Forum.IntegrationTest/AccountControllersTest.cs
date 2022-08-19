
using FluentAssertions;
using Forum.Entities;
using Forum.Exceptions;
using Forum.IntegrationTest.Helper;
using Forum.Models;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;



namespace Forum.IntegrationTest
{
    public class AccountControllersTest : AccountTestsHelper
    {


        public AccountControllersTest() : base()
        {

        }


        [Fact]
        public async Task Create_ForValidModel_ReturnsOk()
        {
            //arrange
            await SeedUsers();
            var user = new CreateUserDto()
            {
                
                Nick = "Test",
                Email = "test@test.com",
                Password = "test1"
            };

            //act
            var httpContent = user.SerializeForHttp();
            var response = await _client.PostAsync("/api/account/register", httpContent);

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
            var response = await _client.PostAsync("/api/account/register", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Login_ForValidModel_ReturnsOk()
        {
            //arrange
            await SeedUsers();
            var model = new LoginDto()
            {
                Nick = "Test123",
                Password = "test1"
            };
            //act
            var httpContent = model.SerializeForHttp();
            var response = await _client.PostAsync("/api/account/login", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Login_ForInvalidModel_ReturnsBadRequest()
        {
            //arrange
            await SeedUsers();
            var model = new LoginDto()
            {
                Nick = "Test"

            };
            //act
            var httpContent = model.SerializeForHttp();
            var response = await _client.PostAsync("/api/account/login", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task GetById_ForInvalidId_ReturnsNotFound()
        {
            await SeedUsers();

            var accountId = 12;

            //act
            var response = await _client.GetAsync($"/api/account/{accountId}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        }
        [Fact]
        public async Task GetById_ForValidId_ReturnsOk()
        {
            await SeedUsers();

            var accountId =2;

            //act
            var response = await _client.GetAsync($"/api/account/{accountId}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }
        [Fact]
        public async Task Delete_ForCorrectId_ReturnsOk()
        {
            //Arrange

            int accountId = 3;
            //Seed
            await SeedUsers();
            //Act
            var response = await _client.DeleteAsync($"api/account/delete/{accountId}");

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Delete_ForIncorrectId_ReturnsNotFound()
        {
            //Arrange
            int accountId = 15;
            await SeedUsers();

            //Seed

            //Act
            var response = await _client.DeleteAsync($"api/account/delete/{accountId}");

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }


    }
}
    
