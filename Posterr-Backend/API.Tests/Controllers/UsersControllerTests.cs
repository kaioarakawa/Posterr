using API.Controllers;
using Application.DTOs;
using Application.UseCases;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly Mock<GetUsersHandler> _mockGetUsersHandler;
        private readonly Mock<GetUserHandler> _mockGetUserHandler;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPostRepository = new Mock<IPostRepository>();
            _mockGetUsersHandler = new Mock<GetUsersHandler>(_mockUserRepository.Object, _mockPostRepository.Object);
            _mockGetUserHandler = new Mock<GetUserHandler>(_mockUserRepository.Object, _mockPostRepository.Object);
            _controller = new UsersController(_mockUserRepository.Object, _mockGetUsersHandler.Object, _mockGetUserHandler.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new UserDto { Id = userId, Name = "John Doe" };
            _mockGetUserHandler.Setup(h => h.Handle(userId)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUser = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(userId, returnUser.Id);
            Assert.Equal("John Doe", returnUser.Name);
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithUsers()
        {
            // Arrange
            var users = new List<UserDto>
        {
            new UserDto { Id = Guid.NewGuid(), Name = "John Doe" },
            new UserDto { Id = Guid.NewGuid(), Name = "Jane Doe" }
        };
            _mockGetUsersHandler.Setup(h => h.Handle()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUsers = Assert.IsType<List<UserDto>>(okResult.Value);
            Assert.Equal(2, returnUsers.Count);
        }
    }
}
