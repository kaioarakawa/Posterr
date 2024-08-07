﻿using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Application.Tests.UseCases
{
    public class GetPostsHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsPostDtos()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(repo => repo.GetPostsAsync(0, 10, "latest", null, null))
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
            Assert.NotNull(result);
            Assert.Equal(2, result.Posts.Count);
            Assert.Equal("Post 1", result.Posts[0].Content);
            Assert.Equal("user1", result.Posts[0].User.Username);
            Assert.Equal("Post 2", result.Posts[1].Content);
            Assert.Equal("user2", result.Posts[1].User.Username);
        }

        [Fact]
        public async Task Handle_EmptyResult_ReturnsEmptyList()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(repo => repo.GetPostsAsync(0, 10, "latest", null, null))
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
            Assert.NotNull(result);
            Assert.Empty(result.Posts);
        }

        [Fact]
        public async Task Handle_SkipAndTakeValues_ReturnsCorrectNumberOfItems()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(repo => repo.GetPostsAsync(10, 5, "latest", null, null))
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
            Assert.NotNull(result);
            Assert.Equal(2, result.Posts.Count); // Ensure it returns exactly 2 items as per mock data
        }

        [Fact]
        public async Task Handle_NullSortBy_ReturnsDefaultSortOrder()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(repo => repo.GetPostsAsync(0, 10, "latest", null, null))
                         .ReturnsAsync(new List<Post>
                         {
                             new Post { Id = 1, Content = "Post 1", User = new User { Username = "user1" }, CreatedAt = DateTime.Now },
                             new Post { Id = 2, Content = "Post 2", User = new User { Username = "user2" }, CreatedAt = DateTime.Now.AddMinutes(-1) }
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
            Assert.NotNull(result);
            Assert.Equal(2, result.Posts.Count);
            Assert.Equal("Post 1", result.Posts[0].Content);
            Assert.Equal("user1", result.Posts[0].User.Username);
            Assert.Equal("Post 2", result.Posts[1].Content);
            Assert.Equal("user2", result.Posts[1].User.Username);
        }

        [Fact]
        public async Task Handle_WithKeyword_ReturnsFilteredResults()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            mockRepository.Setup(repo => repo.GetPostsAsync(0, 10, "latest", "keyword", null))
                         .ReturnsAsync(new List<Post>
                         {
                             new Post { Id = 1, Content = "Post with keyword", User = new User { Username = "user1" } },
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
            Assert.NotNull(result);
            Assert.Single(result.Posts); // Only one post should match the keyword
            Assert.Equal("Post with keyword", result.Posts[0].Content);
            Assert.Equal("user1", result.Posts[0].User.Username);
        }
    }
}
