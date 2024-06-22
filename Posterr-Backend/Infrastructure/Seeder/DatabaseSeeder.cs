using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Seeder
{
    public class DatabaseSeeder : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await context.Database.MigrateAsync(cancellationToken);
                await SeedDatabase(context);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task SeedDatabase(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Id = Guid.NewGuid(), Name = "user1", Username = "user1", CreatedAt = DateTime.UtcNow },
                    new User { Id = Guid.NewGuid(), Name = "user2", Username = "user2", CreatedAt = DateTime.UtcNow },
                    new User { Id = Guid.NewGuid(), Name = "user3", Username = "user3", CreatedAt = DateTime.UtcNow },
                    new User { Id = Guid.NewGuid(), Name = "user4", Username = "user4", CreatedAt = DateTime.UtcNow }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
