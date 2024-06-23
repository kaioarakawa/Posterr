using Application.DTOs;
using Domain.Interfaces;

namespace Application.UseCases
{
    public class GetUsersHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public GetUsersHandler(IUserRepository userRepository, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        public virtual async Task<List<UserDto>> Handle()
        {
            var users = await _userRepository.GetUsersAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var totalPosts = await _postRepository.GetUserPostCountAsync(user.Id);

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    TotalPosts = totalPosts,
                    CreatedAt = user.CreatedAt
                });
            }

            return userDtos;
        }
    }
}
