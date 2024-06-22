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

        public async Task<List<Post>> GetPostsAsync(int skip, int take, string sortBy, string keyword)
        {
            var query = _context.Posts.AsQueryable();
           
            query = query.Include(p => p.User).Include(p => p.OriginalPost);

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

        public async Task<int> GetUserPostCountAsync(Guid userId)
        {
            return await _context.Posts.CountAsync(p => p.UserId == userId && p.CreatedAt >= DateTime.UtcNow.AddDays(-1));
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
                .Include(p => p.User) // Include related user entity if needed
                .Include(p => p.OriginalPost) // Include original post entity if needed
                .FirstOrDefaultAsync(p => p.Id == id);

            return post; // Will return null if post with specified id is not found
        }
    }
}
