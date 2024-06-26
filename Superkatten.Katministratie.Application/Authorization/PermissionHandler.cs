using Microsoft.AspNetCore.Authorization;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Authorization;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserAuthorisationRepository _userAuthorisationRepository;

    public PermissionHandler(IUserAuthorisationRepository userAuthorisationRepository)
    {
        _userAuthorisationRepository = userAuthorisationRepository 
            ?? throw new ArgumentNullException(nameof(userAuthorisationRepository));
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User is null)
        {
            // no user authorized
            context.Fail();
            return Task.CompletedTask;
        }

        var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "id") ?? throw new Exception("No claim with user id found");
        var user = _userAuthorisationRepository.GetUserById(int.Parse(userIdClaim.Value)) ?? throw new Exception($"User not found with id {userIdClaim.Value}");
        
        var hasPermission = user.Permissions.Contains(requirement.Permission);
        if (hasPermission)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
