using Application.DTOs;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class GetPostsRequest
    {
        [Range(0, int.MaxValue, ErrorMessage = "Skip cannot be negative.")]
        public int Skip { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Take must be greater than zero.")]
        public int Take { get; set; }
        public string SortBy { get; set; }
        public string? Keyword { get; set; }
        public Guid? UserId { get; set; }
    }

    public class GetPostsHandler
    {
        private readonly IPostRepository _postRepository;

        public GetPostsHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<GetPostsResponse> Handle(GetPostsRequest request)
        {
            // Get total count of posts for pagination
            int totalPosts = await _postRepository.GetTotalPostsCountAsync(request.Keyword, request.UserId, request.SortBy);

            // Get the posts for the current page
            var posts = await _postRepository.GetPostsAsync(request.Skip, request.Take, request.SortBy, request.Keyword, request.UserId);

            var postDtos = new List<PostDto>();

            foreach (var post in posts)
            {
                var postDto = new PostDto
                {
                    Id = post.Id,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt
                };

                postDto.User = new UserDto
                {
                    Id = post.User.Id,
                    Name = post.User.Name,
                    Username = post.User.Username,
                    TotalPosts = 0,
                    CreatedAt = post.User.CreatedAt
                };

                // Check if this post has an original post
                if (post.OriginalPostId.HasValue)
                {
                    // Map the original post to OriginalPost property
                    postDto.OriginalPost = new PostDto
                    {
                        Content = post.OriginalPost.Content,
                        CreatedAt = post.OriginalPost.CreatedAt
                    };

                    postDto.OriginalPost.User = new UserDto
                    {
                        Id = post.OriginalPost.User.Id,
                        Name = post.OriginalPost.User.Name,
                        Username = post.OriginalPost.User.Username,
                        TotalPosts = 0,
                        CreatedAt = post.OriginalPost.User.CreatedAt
                    };
                }

                postDtos.Add(postDto);
            }

            // Calculate current page based on skip and take
            int currentPage = (request.Skip / request.Take) + 1;

            return new GetPostsResponse
            {
                CurrentPage = currentPage,
                TotalPosts = totalPosts,
                Posts = postDtos
            };
        }
    }
}
