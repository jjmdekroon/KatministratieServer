using Microsoft.IdentityModel.Tokens;
using Superkatten.Katministratie.Application.Configuration;
using Superkatten.Katministratie.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Superkatten.Katministratie.Application.Authenticate;

public class JwtUtils : IJwtUtils
{
    public string GenerateToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(UserAuthorisationConfiguration.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = user.ToString(),
            Audience = "User",
            Subject = new ClaimsIdentity(new[] 
            { 
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, PermissionEnum.Viewer.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token, string? user)
    {
        if (string.IsNullOrEmpty(token))
        {
            return default;
        }
        
        if (string.IsNullOrEmpty(user))
        {
            return default;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(UserAuthorisationConfiguration.Secret);
        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidIssuer = user,
                ValidAudience = "User",
                IssuerSigningKey = new SymmetricSecurityKey(key),
            }, out var validatedToken);

            // return user id from JWT token if validation successful
            return claimsPrincipal;
        }
        catch (SecurityTokenExpiredException)
        {
            throw new ApplicationException("Token has expired.");
        }
        catch(Exception ex)
        {
            throw new ApplicationException("Unknown error", ex);
        }
    }
}