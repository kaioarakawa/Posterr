using API.Controllers;
using Application.DTOs;
using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests.Controllers
{
    public class PostsControllerTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly Mock<CreatePostHandler> _mockCreatePostHandler;
        private readonly Mock<GetPostsHandler> _mockGetPostsHandler;
        private readonly PostsController _controller;

        public PostsControllerTests()
        {
            _mockPostRepository = new Mock<IPostRepository>();
            _mockCreatePostHandler = new Mock<CreatePostHandler>(_mockPostRepository.Object);
            _mockGetPostsHandler = new Mock<GetPostsHandler>(_mockPostRepository.Object);
            _controller = new PostsController(_mockPostRepository.Object, _mockCreatePostHandler.Object, _mockGetPostsHandler.Object);
        }

        [Fact]
        public async Task GetPostById_ReturnsOkResult_WithPost()
        {
            // Arrange
            var postId = 1;
            var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Username = "johndoe", CreatedAt = DateTime.UtcNow };
            var post = new Post { Id = postId, Content = "Post content", CreatedAt = DateTime.UtcNow, User = user };
            _mockPostRepository.Setup(repo => repo.GetPostByIdAsync(postId)).ReturnsAsync(post);

            // Act
            var result = await _controller.GetPostById(postId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnPost = Assert.IsType<PostDto>(okResult.Value);
            Assert.Equal(post.Content, returnPost.Content);
            Assert.Equal(post.User.Id, returnPost.User.Id);
            Assert.Equal(post.User.Name, returnPost.User.Name);
            Assert.Equal(post.User.Username, returnPost.User.Username);
        }

        [Fact]
        public async Task GetPostById_ReturnsNotFound_WhenPostDoesNotExist()
        {
            // Arrange
            var postId = 1;
            _mockPostRepository.Setup(repo => repo.GetPostByIdAsync(postId)).ReturnsAsync((Post)null);

            // Act
            var result = await _controller.GetPostById(postId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreatePost_ReturnsOkResult()
        {
            // Arrange
            var request = new CreatePostRequest { Content = "New post content", UserId = Guid.NewGuid() };
            _mockCreatePostHandler.Setup(handler => handler.Handle(request)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreatePost(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CreatePost_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var request = new CreatePostRequest { Content = "New post content", UserId = Guid.NewGuid() };
            _mockCreatePostHandler.Setup(handler => handler.Handle(request)).Throws(new Exception("An error occurred"));

            // Act
            var result = await _controller.CreatePost(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred", badRequestResult.Value);
        }

        [Fact]
        public async Task GetPosts_ReturnsOkResult_WithPosts()
        {
            // Arrange
            var request = new GetPostsRequest();
            var posts = new List<PostDto>
            {
                new PostDto { Content = "Post 1", CreatedAt = DateTime.UtcNow },
                new PostDto { Content = "Post 2", CreatedAt = DateTime.UtcNow }
            };

            var getPostsResponseDTOs = new GetPostsResponseDTO
            {
                CurrentPage = 1, TotalPosts = posts.Count, Posts = posts
            };

            _mockGetPostsHandler.Setup(handler => handler.Handle(request)).ReturnsAsync(getPostsResponseDTOs);

            // Act
            var result = await _controller.GetPosts(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnGetPosts = Assert.IsType<GetPostsResponseDTO>(okResult.Value);
            var returnPosts = Assert.IsType<List<PostDto>>(returnGetPosts.Posts);
            Assert.Equal(2, returnPosts.Count);
        }
    }
}
