using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Superkatten.Katministratie.Application.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Authenticate.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils, ISuperSession superSession)
    {
        var authorisationKey = context.Request.Headers["Authorization"].FirstOrDefault();
        var token = authorisationKey?.Split(" ").Last();
        var claimsPrincipal = jwtUtils.ValidateToken(token ?? string.Empty, superSession?.User?.ToString() ?? "d1u2m3m4y");
        if (claimsPrincipal is null)
        {
            // When there is something wrong with the token stop the session immediately
            superSession?.Stop();
        }

        await _next(context);
    }
}
