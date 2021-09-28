using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentitySample.Models;
using IdentitySample.Utils;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentitySample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IdentityDbContext context;
        private readonly JwtSettings jwtSettings;

        public AuthenticationController(IdentityDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            this.context = context;
            this.jwtSettings = jwtSettings.Value;
        }
        
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthModel data)
        {
            var usr = await context.Users.FirstOrDefaultAsync(
                x => x.Email == data.Email && x.Password == data.Password.Sha256());
            if (usr == null)
                return BadRequest();
            var tk = generateJwtToken(usr);
            return Ok(new {tk});
        }
        
        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class AuthModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}