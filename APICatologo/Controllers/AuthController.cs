using APICatologo.DTOs;
using APICatologo.Interfaces;
using APICatologo.Migrations;
using APICatologo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;


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

            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
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

                _ = int.TryParse(_configuration["JWT: RefreshTokenValidityInMinutes"], out int RefreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(RefreshTokenValidityInMinutes);

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });

            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = _userManager.FindByNameAsync(model.UserName!);
            if (userExist != null) { 
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status ="Error", Message = "User already exist!"});
            
            }
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName

            };
             
            var result =await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Create User  fail!" });


            }
            return Ok(new Response {Status =" Secelly", Message="User Created succefully" });
        }

        [HttpPost]
        [Route("RefreshToken")]

        public async Task<IActionResult> RefreshToken(TokenModel model)
        {
            if (model == null) { return BadRequest("Invalid Cliente request"); }

            string? acessToken = model.AcessToken ?? throw new ArgumentNullException(nameof(model));
            string ? refreshToken = model.RefreshToken ?? throw new ArgumentNullException( nameof(model));

            var princinpal = _tokenServices.GetPrincinpalFromExpiredToken(acessToken!, _configuration);
            if (princinpal == null) {
                return BadRequest("Acess token invalid/ refresh token");
            
            }

            string userName = princinpal.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) 
            {
                return BadRequest("Invalid acess token/ refresk token");
            
            }

            var newAessToken = _tokenServices.GenerateAcessToken(princinpal.Claims.ToList(), _configuration);

            var newRefreshtoken = _tokenServices.GenerateRefreshToken();

            user.RefreshToken = newRefreshtoken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {

                acessToken = new JwtSecurityTokenHandler().WriteToken(newAessToken),
                refreshToken = newRefreshtoken,
            } );
        }

        [Authorize]
        [HttpPost]
        [Route("Revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) { return BadRequest(); }

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }

    }
}
