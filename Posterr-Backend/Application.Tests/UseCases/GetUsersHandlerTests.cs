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
    public class GetUsersHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsListOfUserDtos()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPostRepository = new Mock<IPostRepository>();
            
            // Simulate data returned from repository
            var mockUsers = new List<User>
            {
                new User { Id = Guid.NewGuid(), Name = "user1" ,Username = "user1", CreatedAt = DateTime.UtcNow },
                new User { Id = Guid.NewGuid(), Name = "user2" ,Username = "user2", CreatedAt = DateTime.UtcNow }
            };
            mockUserRepository.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(mockUsers);

            mockPostRepository.Setup(repo => repo.GetPostsAsync(0, 10, "latest", null, null))
                         .ReturnsAsync(new List<Post>
                         {
                             new Post { Id = 1, Content = "Post 1", User = new User { Username = "user1" } },
                             new Post { Id = 2, Content = "Post 2", User = new User { Username = "user2" } }
                         });

            var handler = new GetUsersHandler(mockUserRepository.Object, mockPostRepository.Object);

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
