using APICatologo.DTOs;
using APICatologo.Interfaces;
using APICatologo.Migrations;
using APICatologo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatologo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenServices _tokenServices;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityUser> _rolemanager;

        public AuthController(ITokenServices tokenServices,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityUser> rolemanager)
        {
            _tokenServices = tokenServices;
            _configuration = configuration;
            _userManager = userManager;
            _rolemanager = rolemanager;
        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName!);

            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!)) {
                var userRoles = await _userManager.GetRolesAsync(user);
                
                var AuthoClaims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),


            };


                foreach (var userRole in userRoles) 
                {
                    AuthoClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }


                var token = _tokenServices.GenerateAcessToken(AuthoClaims, _configuration);
                var refreshToken = _tokenServices.GenerateRefreshToken();

                _ = int.TryParse(_tokenServices["JWT: RefreshTokenValidityInMinutes"], out int RefreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(RefreshTokenValidityInMinutes);

                await _userManager.UpdateAsync(user);

                return Ok(new {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
                });

            }
        }

    }
}
