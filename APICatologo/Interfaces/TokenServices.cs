using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICatologo.Interfaces
{
    public class TokenServices : ITokenServices
    {
        public JwtSecurityToken GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey")??
                throw new InvalidOperationException("Invalid secret key" );

            var privateKey = Encoding.UTF8.GetBytes(key);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256Signature
                );




        }

        public string GenerateAcessToken()
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincinpal GetPrincinpalFromExpiredToken(string token, IConfiguration _config)
        {
            throw new NotImplementedException();
        }
    }
}
