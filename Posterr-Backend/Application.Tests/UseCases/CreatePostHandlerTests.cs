﻿using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace Application.Tests.UseCases
{
    public class CreatePostHandlerTests
    {
        [Fact]
        public async Task Handle_CreatePost_ValidRequest_CreatesPost()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            var handler = new CreatePostHandler(mockRepository.Object);
            var request = new CreatePostRequest
            {
                UserId = Guid.NewGuid(),
                Content = "Test post content"
            };

            mockRepository.Setup(repo => repo.GetUserPostCountAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                          .ReturnsAsync(0); // Mocking GetUserPostCountAsync to return 0

            // Act
            await handler.Handle(request);

            // Assert
            mockRepository.Verify(repo => repo.AddPostAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CreatePost_DailyLimitExceeded_ThrowsException()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            var handler = new CreatePostHandler(mockRepository.Object);
            var request = new CreatePostRequest
            {
                UserId = Guid.NewGuid(),
                Content = "Test post content"
            };

            // Configurando o mock para retornar 5 posts em qualquer intervalo de tempo (limite excedido)
            mockRepository.Setup(repo => repo.GetUserPostCountAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                          .ReturnsAsync(5);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request));
        }

        [Fact]
        public async Task Handle_CreateRepost_ValidRequest_CreatesRepost()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            var handler = new CreatePostHandler(mockRepository.Object);
            var request = new CreateRepostRequest
            {
                UserId = Guid.NewGuid(),
                Content = "Test repost content",
                PostId = 1 // Assuming PostId exists for testing
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Username = "Test",
                CreatedAt = DateTime.UtcNow,
            };

            mockRepository.Setup(repo => repo.GetUserPostCountAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                          .ReturnsAsync(0); // Mocking GetUserPostCountAsync to return 0

            mockRepository.Setup(repo => repo.GetPostByIdAsync(request.PostId))
                          .ReturnsAsync(new Post { Id = request.PostId, User = user }); // Mocking GetPostByIdAsync to return a post

            mockRepository.Setup(repo => repo.HasUserRepostedAsync(request.UserId, request.PostId))
                          .ReturnsAsync(false); // Mocking HasUserRepostedAsync to return false

            // Act
            await handler.Handle(request);

            // Assert
            mockRepository.Verify(repo => repo.AddPostAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CreateRepost_PostNotExist_ThrowsException()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            var handler = new CreatePostHandler(mockRepository.Object);
            var request = new CreateRepostRequest
            {
                UserId = Guid.NewGuid(),
                Content = "Test repost content",
                PostId = 1 // Assuming PostId does not exist for testing
            };

            mockRepository.Setup(repo => repo.GetPostByIdAsync(request.PostId))
                          .ReturnsAsync((Post)null); // Mocking GetPostByIdAsync to return null (post not found)

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request));
        }

        [Fact]
        public async Task Handle_CreateRepost_AlreadyReposted_ThrowsException()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            var handler = new CreatePostHandler(mockRepository.Object);
            var request = new CreateRepostRequest
            {
                UserId = Guid.NewGuid(),
                Content = "Test repost content",
                PostId = 1 // Assuming PostId exists for testing
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Username = "Test",
                CreatedAt = DateTime.UtcNow,
            };

            mockRepository.Setup(repo => repo.GetPostByIdAsync(request.PostId))
                          .ReturnsAsync(new Post { Id = request.PostId, User = user }); // Mocking GetPostByIdAsync to return a post

            mockRepository.Setup(repo => repo.HasUserRepostedAsync(request.UserId, request.PostId))
                          .ReturnsAsync(true); // Mocking HasUserRepostedAsync to return true (already reposted)

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request));
        }

        [Fact]
        public async Task Handle_CreateRepost_PostedByUser_ThrowsException()
        {
            // Arrange
            var mockRepository = new Mock<IPostRepository>();
            var handler = new CreatePostHandler(mockRepository.Object);
            var request = new CreateRepostRequest
            {
                UserId = Guid.NewGuid(),
                Content = "Test repost content",
                PostId = 1 // Assuming PostId exists for testing
            };

            var user = new User
            {
                Id = request.UserId,
                Name = "Test",
                Username = "Test",
                CreatedAt = DateTime.UtcNow,
            };

            mockRepository.Setup(repo => repo.GetPostByIdAsync(request.PostId))
                          .ReturnsAsync(new Post { Id = request.PostId, User = user }); // Mocking GetPostByIdAsync to return a post

            mockRepository.Setup(repo => repo.HasUserRepostedAsync(request.UserId, request.PostId))
                          .ReturnsAsync(true); // Mocking HasUserRepostedAsync to return true (already reposted)

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request));
        }
    }
}
