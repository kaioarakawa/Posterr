using Infrastructure.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.Tests.Data
{
    public class PostRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public PostRepositoryTests()
        {
            // Initialize DbContextOptions with in-memory database provider
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetPostsAsync_ReturnsPostsCorrectly()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                var repository = new PostRepository(context);

                context.Posts.AddRange(
                    new Post { Id = 1, Content = "Post 1", CreatedAt = DateTime.UtcNow },
                    new Post { Id = 2, Content = "Post 2", CreatedAt = DateTime.UtcNow.AddHours(-1) },
                    new Post { Id = 3, Content = "Post 3", CreatedAt = DateTime.UtcNow.AddHours(-2) }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new PostRepository(context);

                // Act
                var posts = await repository.GetPostsAsync(0, 10, "createdAt", null);

                // Assert
                Assert.Equal(3, posts.Count);
                Assert.Equal("Post 1", posts[0].Content);
                Assert.Equal("Post 2", posts[1].Content);
                Assert.Equal("Post 3", posts[2].Content);
            }
        }

        [Fact]
        public async Task GetUserPostCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                var repository = new PostRepository(context);

                context.Posts.AddRange(
                    new Post { Id = 1, UserId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow },
                    new Post { Id = 2, UserId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddHours(-25) },
                    new Post { Id = 3, UserId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddHours(-2) }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new PostRepository(context);

                // Act
                var user = new User { Id = Guid.NewGuid() }; // Simulate user
                var count = await repository.GetUserPostCountAsync(user.Id);

                // Assert
                Assert.Equal(2, count); // Only 2 posts within the last 24 hours
            }
        }

        [Fact]
        public async Task AddPostAsync_AddsPostCorrectly()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                var repository = new PostRepository(context);
                var post = new Post { Id = 1, Content = "New Post", UserId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow };

                // Act
                await repository.AddPostAsync(post);
            }

            using (var context = new AppDbContext(_options))
            {
                // Assert
                var addedPost = await context.Posts.FirstOrDefaultAsync(p => p.Id == 1);
                Assert.NotNull(addedPost);
                Assert.Equal("New Post", addedPost.Content);
            }
        }

        [Fact]
        public async Task HasUserRepostedAsync_ReturnsTrueIfReposted()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                var repository = new PostRepository(context);

                var userId = Guid.NewGuid();
                var postId = 1;

                context.Posts.Add(new Post { Id = postId, UserId = userId, OriginalPostId = null });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new PostRepository(context);

                // Act
                var hasReposted = await repository.HasUserRepostedAsync(Guid.NewGuid(), 1);

                // Assert
                Assert.True(hasReposted);
            }
        }

        [Fact]
        public async Task GetPostByIdAsync_ReturnsPostIfExists()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                var repository = new PostRepository(context);

                var postId = 1;
                context.Posts.Add(new Post { Id = postId, Content = "Test Post" });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new PostRepository(context);

                // Act
                var post = await repository.GetPostByIdAsync(1);

                // Assert
                Assert.NotNull(post);
                Assert.Equal("Test Post", post.Content);
            }
        }

        [Fact]
        public async Task GetPostsAsync_ReturnsTrendingPosts()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                var repository = new PostRepository(context);

                var originalPostId = 1;
                context.Posts.AddRange(
                    new Post { Id = 1, Content = "Post 1", CreatedAt = DateTime.UtcNow },
                    new Post { Id = 2, Content = "Post 2", CreatedAt = DateTime.UtcNow.AddHours(-1) },
                    new Post { Id = 3, Content = "Post 3", CreatedAt = DateTime.UtcNow.AddHours(-2), OriginalPostId = originalPostId },
                    new Post { Id = 4, Content = "Post 4", CreatedAt = DateTime.UtcNow.AddHours(-3), OriginalPostId = originalPostId },
                    new Post { Id = 5, Content = "Post 5", CreatedAt = DateTime.UtcNow.AddHours(-4), OriginalPostId = originalPostId }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new PostRepository(context);

                // Act
                var posts = await repository.GetPostsAsync(0, 10, "trending", null);

                // Assert
                Assert.Equal(3, posts.Count);
                Assert.Equal("Post 5", posts[0].Content); // Most reposted post should come first
            }
        }
    }
}
