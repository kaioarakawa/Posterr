using Domain.Entities;
using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases
{
    public class CreatePostRequest
    {
        [Required(ErrorMessage = "User Id is required.")]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(777, ErrorMessage = "Content exceeds maximum length of 777 characters.")]
        public string Content { get; set; }
    }

    public class CreateRepostRequest
    {
        [Required(ErrorMessage = "User Id is required.")]
        public Guid UserId { get; set; }

        [MaxLength(777, ErrorMessage = "Content exceeds maximum length of 777 characters.")]
        public string? Content { get; set; }
        [Required]
        public int PostId { get; set; }
    }

    public class CreatePostHandler
    {
        private readonly IPostRepository _postRepository;

        public CreatePostHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task Handle(CreatePostRequest request)
        {
            // Check if user has already posted or reposted more than 5 times today
            if (await _postRepository.GetUserPostCountAsync(request.UserId) >= 5)
            {
                throw new Exception("Daily post limit exceeded.");
            }

            var post = new Post
            {
                UserId = request.UserId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                OriginalPost = null 
            };

            await _postRepository.AddPostAsync(post);
        }

        public async Task Handle(CreateRepostRequest request)
        {
            // Check if user has already posted or reposted more than 5 times today
            if (await _postRepository.GetUserPostCountAsync(request.UserId) >= 5)
            {
                throw new Exception("Daily post limit exceeded.");
            }

            var post = await _postRepository.GetPostByIdAsync(request.PostId);

            if (post == null)
            {
                throw new Exception("This post not exist to repost them.");
            }

            // Check if user has already reposted this post
            var hasReposted = await _postRepository.HasUserRepostedAsync(request.UserId, request.PostId);
            if (hasReposted)
            {
                throw new Exception("You have already reposted this post.");
            }

            var repost = new Post
            {
                UserId = request.UserId,
                Content = request.Content,
                OriginalPostId = request.PostId,// Indicates this is a repost
                CreatedAt = DateTime.UtcNow
            };

            await _postRepository.AddPostAsync(repost);
        }
    }
}
