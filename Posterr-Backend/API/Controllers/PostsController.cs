using Microsoft.AspNetCore.Mvc;
using Application.UseCases;
using Application.DTOs;
using Domain.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly CreatePostHandler _createPostHandler;
        private readonly GetPostsHandler _getPostsHandler;
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository, CreatePostHandler createPostHandler, GetPostsHandler getPostsHandler)
        {
            _postRepository = postRepository;
            _createPostHandler = createPostHandler;
            _getPostsHandler = getPostsHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound(); // Return 404 Not Found if post with id is not found
            }

            var postDto = new PostDto
            {
                Username = post.User.Username,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
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

            return Ok(postDto); // Return the mapped DTO
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            try
            {
                await _createPostHandler.Handle(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/repost")]
        public async Task<IActionResult> RepostPost([FromBody] CreateRepostRequest request)
        {
            try
            {
                await _createPostHandler.Handle(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] GetPostsRequest request)
        {
            var posts = await _getPostsHandler.Handle(request);
            return Ok(posts);
        }
    }
}
