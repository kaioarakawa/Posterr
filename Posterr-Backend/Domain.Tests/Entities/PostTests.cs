using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests.Entities
{
    public class PostTests
    {
        [Fact]
        public void Post_ConstructsCorrectly()
        {
            // Arrange
            string content = "Lorem ipsum dolor sit amet.";
            Guid userId = Guid.NewGuid();
            int? originalPostId = 1; // Example original post ID

            // Act
            var post = new Post
            {
                Content = content,
                UserId = userId,
                OriginalPostId = originalPostId
            };

            // Assert
            Assert.Equal(content, post.Content);
            Assert.Equal(userId, post.UserId);
            Assert.Equal(originalPostId, post.OriginalPostId);
        }

        [Fact]
        public void Post_ContentMaxLengthValidation()
        {
            // Arrange
            var post = new Post();
            string contentExceedingMaxLength = new string('A', 778); // Create a string longer than 777 characters

            // Act
            post.Content = contentExceedingMaxLength;

            // Assert
            Assert.NotNull(post.Content); // Content should not be null due to automatic property initialization
            Assert.True(post.Content.Length <= 777); // Content should respect the max length constraint
        }

        [Fact]
        public void Post_Relationships_User()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "testuser" };
            var post = new Post { UserId = user.Id, User = user };

            // Act & Assert
            Assert.Equal(user.Id, post.UserId);
            Assert.Equal(user, post.User);
        }

        [Fact]
        public void Post_Relationships_OriginalPost()
        {
            // Arrange
            var originalPost = new Post { Id = 1 };
            var repost = new Post { OriginalPostId = originalPost.Id, OriginalPost = originalPost };

            // Act & Assert
            Assert.Equal(originalPost.Id, repost.OriginalPostId);
            Assert.Equal(originalPost, repost.OriginalPost);
        }

        [Fact]
        public void Post_ContentNullValidation()
        {
            // Arrange
            var post = new Post();

            // Act
            post.Content = null;

            // Assert
            Assert.Null(post.Content); // Content can be null if not required
        }
    }
}