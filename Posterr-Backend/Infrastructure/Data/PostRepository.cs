// Infrastructure/Data/PostRepository.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalPostsCountAsync(string? keyword, Guid? userId)
        {
            var query = _context.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.Content.Contains(keyword));
            }

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId == userId);
            }

            return await query.CountAsync();
        }

        public async Task<List<Post>> GetPostsAsync(int skip, int take, string sortBy, string keyword, Guid? userId = null)
        {
            var query = _context.Posts.AsQueryable();

            query = query.Include(p => p.User)
                 .Include(p => p.OriginalPost)
                    .ThenInclude(op => op.User);

            if (userId.HasValue)
            {
                query = query.Where(p => p.User.Id == userId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.Content.Contains(keyword));
            }

            if (sortBy == "trending")
            {
                // Order by number of reposts for trending posts
                query = query.OrderByDescending(p => _context.Posts.Count(rp => rp.OriginalPostId == p.Id));
            }
            else
            {
                // Order by CreatedAt for latest posts
                query = query.OrderByDescending(p => p.CreatedAt);
            }

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> GetUserPostCountAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            IQueryable<Post> query = _context.Posts.Where(p => p.UserId == userId);

            if (startDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt <= endDate.Value);
            }

            return await query.CountAsync();
        }

        public async Task AddPostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> HasUserRepostedAsync(Guid userId, int postId)
        {
            return await _context.Posts.AnyAsync(r => r.UserId == userId && r.OriginalPostId == postId);
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.OriginalPost)
                .FirstOrDefaultAsync(p => p.Id == id);

            return post;
        }
    }
}
