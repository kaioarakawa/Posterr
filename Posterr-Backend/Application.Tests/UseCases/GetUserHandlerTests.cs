using Application.DTOs;
using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.UseCases
{
    public class GetUserHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsListOfUserDtos()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();

            // Simulate data returned from repository
            var mockUsers = new List<User>
            {
                new User { Id = Guid.NewGuid(), Name = "user1" ,Username = "user1", CreatedAt = DateTime.UtcNow },
                new User { Id = Guid.NewGuid(), Name = "user2" ,Username = "user2", CreatedAt = DateTime.UtcNow }
            };
            mockUserRepository.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(mockUsers);

            var handler = new GetUserHandler(mockUserRepository.Object);

            // Act
            var result = await handler.Handle();

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(2, result.Count);

            // Additional assertions if needed
            Xunit.Assert.Equal(mockUsers[0].Id, result[0].Id);
            Xunit.Assert.Equal(mockUsers[1].Username, result[1].Username);
        }
    }
}
