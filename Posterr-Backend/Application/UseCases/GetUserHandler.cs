using Application.DTOs;
using Domain.Interfaces;

namespace Application.UseCases
{
    public class GetUserHandler
    {
        private readonly IUserRepository _userRepository;

        public GetUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> Handle()
        {
            var users = await _userRepository.GetUsersAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                CreatedAt = u.CreatedAt
            }).ToList();
        }
    }
}
