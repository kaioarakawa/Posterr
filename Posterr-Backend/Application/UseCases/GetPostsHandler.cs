using Application.DTOs;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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
    }

    public class GetPostsHandler
    {
        private readonly IPostRepository _postRepository;

        public GetPostsHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<List<PostDto>> Handle(GetPostsRequest request)
        {
            var posts = await _postRepository.GetPostsAsync(request.Skip, request.Take, request.SortBy, request.Keyword);

            var postDtos = new List<PostDto>();

            foreach (var post in posts)
            {
                var postDto = new PostDto
                {
                    Id = post.Id,
                    Username = post.User.Username,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt
                };

                // Check if this post has an original post
                if (post.OriginalPostId.HasValue)
                {
                    // Map the original post to OriginalPost property
                    postDto.OriginalPost = new PostDto
                    {
                        Username = post.OriginalPost.User.Username,
                        Content = post.OriginalPost.Content,
                        CreatedAt = post.OriginalPost.CreatedAt
                    };
                }

                postDtos.Add(postDto);
            }

            return postDtos;
        }
    }
}
