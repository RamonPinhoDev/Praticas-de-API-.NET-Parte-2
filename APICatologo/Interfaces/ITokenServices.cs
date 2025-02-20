﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatologo.Interfaces
{
    public interface ITokenServices
    {
        JwtSecurityToken GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration _config);
        string GenerateAcessToken();

        ClaimsPrincinpal GetPrincinpalFromExpiredToken(string token, IConfiguration _config);
    }
}
