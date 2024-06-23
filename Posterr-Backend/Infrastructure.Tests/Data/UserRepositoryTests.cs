using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Data
{
    public class UserRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public UserRepositoryTests()
        {
            // Initialize DbContextOptions with in-memory database provider
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsUsersCorrectly()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                context.Users.AddRange(
                    new User { Id = Guid.NewGuid(), Name = "teste", Username = "user1", CreatedAt = DateTime.UtcNow },
                    new User { Id = Guid.NewGuid(), Name = "teste", Username = "user2", CreatedAt = DateTime.UtcNow }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new UserRepository(context);

                // Act
                var users = await repository.GetUsersAsync();

                // Assert
                Assert.Equal(2, users.Count);
                Assert.Equal("user1", users[0].Username);
                Assert.Equal("user2", users[1].Username);
            }
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUserIfExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Name = "teste", Username = "testuser", CreatedAt = DateTime.UtcNow };

            using (var context = new AppDbContext(_options))
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new UserRepository(context);

                // Act
                var retrievedUser = await repository.GetUserByIdAsync(userId);

                // Assert
                Assert.NotNull(retrievedUser);
                Assert.Equal(userId, retrievedUser.Id);
                Assert.Equal("testuser", retrievedUser.Username);
            }
        }
    }
}
