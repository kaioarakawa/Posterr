using Application.DTOs;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class GetUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public GetUserHandler(IUserRepository userRepository, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        public virtual async Task<UserDto> Handle(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id) ?? throw new Exception("User not found");
            var totalPosts = await _postRepository.GetUserPostCountAsync(user.Id);

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                TotalPosts = totalPosts,
                CreatedAt = user.CreatedAt,
            };

            return userDto;
        }
    }
}
