using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests.Entities
{
    public class UserTests
    {
        [Fact]
        public void User_ConstructsCorrectly()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string username = "john_doe";
            DateTime createdAt = DateTime.UtcNow;

            // Act
            var user = new User
            {
                Id = id,
                Username = username,
                CreatedAt = createdAt
            };

            // Assert
            Assert.Equal(id, user.Id);
            Assert.Equal(username, user.Username);
            Assert.Equal(createdAt, user.CreatedAt);
        }
    }
}
