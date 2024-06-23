using Application.DTOs;
using Application.UseCases;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly GetUsersHandler _getUsersHandler;
        private readonly GetUserHandler _getUserHandler;
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository, GetUsersHandler getUsersHandler, GetUserHandler getUserHandler)
        {
            _userRepository = userRepository;
            _getUsersHandler = getUsersHandler;
            _getUserHandler = getUserHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _getUserHandler.Handle(id);
            return Ok(user); 
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _getUsersHandler.Handle();
            return Ok(users);
        }
    }
}
