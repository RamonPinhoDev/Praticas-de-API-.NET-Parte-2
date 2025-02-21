using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
                //algoritimo de encripitação
                SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescripto = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),
                Audience = _config.GetValue<string>("ValisAudience"),
                Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),
                SigningCredentials = signingCredentials

            }; 

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescripto);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(secureRandomBytes);
            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }

        public ClaimsPrincipal GetPrincinpalFromExpiredToken(string token, IConfiguration _config)
        {
            var secretykey = _config["JTW:Securytikey"] ?? throw new InvalidOperationException("Invalid key");
            var tokenValidationParamters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretykey)),
                ValidateLifetime = false,

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var princinpal = tokenHandler.ValidateToken(token, tokenValidationParamters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms
                .HmacSha256, StringComparison.InvariantCultureIgnoreCase) )
                {

                throw new SecurityTokenException("Invalid Key");
            }
            return princinpal;
        }
    }
}
