using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using UsersAPI.Models;
using UsersAPI.Repository;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsersDAL _usersDAL;
        private readonly IConfiguration _congif;
        public LoginController(IUsersDAL usersDAL, IConfiguration congif)
        {
            _usersDAL = usersDAL;
            _congif = congif;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
        {
            var user = await _usersDAL.ValidateUser(request);
            if (user != null)
            {
                return Ok(new { token= await TokenGenerator(user) } );
            }
            else
            {
                return  Unauthorized(new { message = "Invalid credentials" });
            }
        }
        private async Task<string> TokenGenerator( User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_congif["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    _congif["Jwt:Issuer"],
                    _congif["Jwt:Audience"],
                    new Claim[]{
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, (user.IsAdmin == true ? "Admin" : "User"))
                    },
                    null,
                    DateTime.Now.AddMinutes(60),
                    credentials
                    );
            return await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token)); ;
        }
    }
}