using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPostRepository
    {
        Task AddPostAsync(Post post);
        Task<int> GetUserPostCountAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<List<Post>> GetPostsAsync(int skip, int take, string sortBy, string keyword, Guid? userId = null);
        Task<int> GetTotalPostsCountAsync(string? keyword, Guid? userId);
        Task<Post?> GetPostByIdAsync(int id);
        Task<bool> HasUserRepostedAsync(Guid userId, int postId);
    }
}
