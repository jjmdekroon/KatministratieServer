using Superkatten.Katministratie.Domain.Entities;
using System.Security.Claims;

namespace Superkatten.Katministratie.Application.Authenticate;

public interface IJwtUtils
{
    public string GenerateToken(User user);
    public ClaimsPrincipal? ValidateToken(string token, string user);
}