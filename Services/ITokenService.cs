using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiWow.Services
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccesToken(IEnumerable<Claim> claims, IConfiguration _config);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
    }
}
