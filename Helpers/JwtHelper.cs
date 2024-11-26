using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace real_estate_api.Helpers
{
    public class JwtHelper
    {
        private readonly string _secretKey;

        public JwtHelper(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Key"];
        }
        public string GenerateJwtToken(string username, string userId)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, userId)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "yourIssuer",
                audience: "yourAudience",
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Token expires in 1 day
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }
}
