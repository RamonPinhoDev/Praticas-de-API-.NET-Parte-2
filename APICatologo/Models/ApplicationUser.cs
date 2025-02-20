using APICatologo.Migrations;
using Microsoft.AspNetCore.Identity;

namespace APICatologo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
