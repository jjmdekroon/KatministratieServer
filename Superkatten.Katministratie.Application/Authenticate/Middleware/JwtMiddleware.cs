using Microsoft.AspNetCore.Http;
using Superkatten.Katministratie.Application.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Authenticate.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var authorisationKey = context.Request.Headers["Authorization"].FirstOrDefault();
        var token = authorisationKey?.Split(" ").Last();

        var userId = jwtUtils.ValidateToken(token ?? string.Empty);
        if (userId is not null)
        {
            // attach user to context on successful jwt validation
            context.Items["user"] = userService.GetById(userId.Value);
        }

        await _next(context);
    }
}
