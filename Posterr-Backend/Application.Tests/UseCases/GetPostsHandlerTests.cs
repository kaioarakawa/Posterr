using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace Application.Tests.UseCases
{
    public class GetPostsHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsPostDtos()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(repo => repo.GetPostsAsync(0, 10, "latest", null))
                         .ReturnsAsync(new List<Post>
                         {
                             new Post { Id = 1, Content = "Post 1", User = new User { Username = "user1" } },
                             new Post { Id = 2, Content = "Post 2", User = new User { Username = "user2" } }
                         });

            var handler = new GetPostsHandler(mockRepository.Object);
            var request = new GetPostsRequest
            {
                Skip = 0,
                Take = 10,
                SortBy = "latest",
                Keyword = null
            };

            // Act
            var result = await handler.Handle(request);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(2, result.Count);
            Xunit.Assert.Equal("Post 1", result[0].Content);
            Xunit.Assert.Equal("user1", result[0].Username);
            Xunit.Assert.Equal("Post 2", result[1].Content);
            Xunit.Assert.Equal("user2", result[1].Username);
        }

        [Fact]
        public async Task Handle_EmptyResult_ReturnsEmptyList()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(repo => repo.GetPostsAsync(0, 10, "latest", null))
                         .ReturnsAsync(new List<Post>());

            var handler = new GetPostsHandler(mockRepository.Object);
            var request = new GetPostsRequest
            {
                Skip = 0,
                Take = 10,
                SortBy = "latest",
                Keyword = null
            };

            // Act
            var result = await handler.Handle(request);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_SkipAndTakeValues_ReturnsCorrectNumberOfItems()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(repo => repo.GetPostsAsync(10, 5, "latest", null))
                         .ReturnsAsync(new List<Post>
                         {
                             new Post { Id = 1, Content = "Post 1", User = new User { Username = "user1" } },
                             new Post { Id = 2, Content = "Post 2", User = new User { Username = "user2" } }
                         });

            var handler = new GetPostsHandler(mockRepository.Object);
            var request = new GetPostsRequest
            {
                Skip = 10,
                Take = 5,
                SortBy = "latest",
                Keyword = null
            };

            // Act
            var result = await handler.Handle(request);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(2, result.Count); // Ensure it returns exactly 2 items as per mock data
        }

        [Fact]
        public async Task Handle_NullSortBy_ReturnsDefaultSortOrder()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(repo => repo.GetPostsAsync(0, 10, "default", null))
                         .ReturnsAsync(new List<Post>
                         {
                             new Post { Id = 1, Content = "Post 1", User = new User { Username = "user1" } },
                             new Post { Id = 2, Content = "Post 2", User = new User { Username = "user2" } }
                         });

            var handler = new GetPostsHandler(mockRepository.Object);
            var request = new GetPostsRequest
            {
                Skip = 0,
                Take = 10,
                SortBy = null, // Simulating null SortBy parameter
                Keyword = null
            };

            // Act
            var result = await handler.Handle(request);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(2, result.Count);
            Xunit.Assert.Equal("Post 1", result[0].Content); // Ensure default sorting behavior is applied
            Xunit.Assert.Equal("user1", result[0].Username);
            Xunit.Assert.Equal("Post 2", result[1].Content);
            Xunit.Assert.Equal("user2", result[1].Username);
        }

        [Fact]
        public async Task Handle_WithKeyword_ReturnsFilteredResults()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(repo => repo.GetPostsAsync(0, 10, "latest", "keyword"))
                         .ReturnsAsync(new List<Post>
                         {
                             new Post { Id = 1, Content = "Post with keyword", User = new User { Username = "user1" } },
                             new Post { Id = 2, Content = "Another post", User = new User { Username = "user2" } }
                         });

            var handler = new GetPostsHandler(mockRepository.Object);
            var request = new GetPostsRequest
            {
                Skip = 0,
                Take = 10,
                SortBy = "latest",
                Keyword = "keyword"
            };

            // Act
            var result = await handler.Handle(request);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Single(result); // Only one post should match the keyword
            Xunit.Assert.Equal("Post with keyword", result[0].Content);
            Xunit.Assert.Equal("user1", result[0].Username);
        }
    }
}
