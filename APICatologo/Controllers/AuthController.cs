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
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;


namespace APICatologo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenServices _tokenServices;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolemanager;

        private readonly ILogger<AuthController> _logger;

        public AuthController(ITokenServices tokenServices,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolemanager,
            ILogger<AuthController> logger)
        {
            _tokenServices = tokenServices;
            _configuration = configuration;
            _userManager = userManager;
            _rolemanager = rolemanager;
            _logger = logger;
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
        [Route("Register")]
        [HttpPost]
        
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = await _userManager.FindByNameAsync(model.UserName!);
            if (userExist != null) { 
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status ="Error", Message = "User already exist!"});
            
            }
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName

            };
             
            var result = await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Create User  fail!" });


            }
            return Ok(new Response {Status =" Secelly", Message="User Created succefully" });
        }
        [Route("RefreshToken")]
        [HttpPost]
        

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

        [HttpPost]
        [Route("Roles")]
        public async Task< IActionResult> CreateRole(string roleName)
        {
            var roleExist = await  _rolemanager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                var roleResult = await  _rolemanager.CreateAsync(new IdentityRole(roleName));
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                    return StatusCode(StatusCodes.Status200OK, new Response {Status ="Error",  Message=$"Role {roleName} added successfully"});
                }
                else
                {
                    _logger.LogInformation(2, "Error");
                    return StatusCode(StatusCodes.Status400BadRequest, new Response {Status ="Error", Message=$"Issue adding new {roleName} role" });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message="Role already exist."});
        }

        [HttpPost]
        [Route("AddUserRole")]
        public async Task<IActionResult> AddUserRole(string email, string rolename)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await  _userManager.AddToRoleAsync(user, rolename);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User{user.Email} to the {rolename} role");
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Sucess", Message = $"User {user} added to the {rolename} role" });

                }
                else 
                {
                    _logger.LogInformation(1, $"Error Unable to add user {user.Email} to the {rolename} role");
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Sucess", Message = $"Error Unable to add user {user}  to the {rolename} role" });




                }

            }

            return BadRequest(new { error = "Unable to find user" });

        }

    }
}
