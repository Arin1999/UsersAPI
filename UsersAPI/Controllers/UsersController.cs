using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UsersAPI.Models;
using UsersAPI.Repository;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersDAL _usersDAL;
        public UsersController(IUsersDAL usersDAL)
        {
            _usersDAL = usersDAL;
        }

        [HttpGet]
        [Route("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<List<ViewUser>> GetUsers()
        {
            return await _usersDAL.GetUsers();
        }

        [HttpGet]
        [Route("GetUser/{userId}")]
        [Authorize]
        public async Task<ActionResult<ViewUser>> GetUser(string userId)
        {
            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest(new { message = "userId is invalid (not uuid)" });
            }
            var user = await _usersDAL.GetUser(parsedUserId);
            if (user == null)
                return NotFound(new { message = "userId doesn't exist" });
            return Ok(user);
        }

        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest(new { message = "userId is invalid (not uuid)" });
            }
            var user = await _usersDAL.DeleteUser(parsedUserId);
            if (user)
            {
                return NoContent();
            }
            else
            {
                return NotFound(new { message = "userId doesn't exist" });
            }
        }

        [HttpPost]
        [Route("AddUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> CreateUser([FromBody] AddUpdateUser request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Required fields missing" });
            }
            if ( await _usersDAL.checkExistUserName(request.Username)){
                return BadRequest(new { message = "Username already exists" });
            }
            var id =await _usersDAL.AddUsers(request);
            return Created("", id);
        }

        [HttpPut]
        [Route("UpdateUser/{userId}")]
        [Authorize]
        public async Task<ActionResult<User>> UpdateUser(string userId, [FromBody] AddUpdateUser request)
        {
            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest(new { message = "userId is invalid (not uuid)" });
            }
            var user = await _usersDAL.GetUser(parsedUserId);
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            if (userName == user.Username || role== "Admin")
            {
                if (user == null)
                {
                    return NotFound(new { message = "userId doesn't exist" });
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(new { message = "Required fields missing" });
                    }
                    if ( await _usersDAL.checkExistUserName(request.Username, parsedUserId))
                    {
                        return BadRequest(new { message = "Username already exists" });
                    }
                    var isUpdate = await _usersDAL.UpdateUsers(parsedUserId, request);
                    return Ok(isUpdate);
                }
            }
            else
            {
                return Forbid();
            }
        }
    }
}
