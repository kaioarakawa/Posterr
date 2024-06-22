using Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly GetUserHandler _getUserHandler;

        public UsersController(GetUserHandler getUserHandler)
        {
            _getUserHandler = getUserHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _getUserHandler.Handle();
            return Ok(users);
        }
    }
}
