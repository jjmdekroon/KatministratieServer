using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Superkatten.Katministratie.Application.Services;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Authorization;

internal class AuthorisationHandler : IAuthorizationHandler
{
    private readonly IUserAuthorisationRepository _userAuthorisationRepository;
    private readonly ISuperSession _superSession;

    public AuthorisationHandler(
        IUserAuthorisationRepository userAuthorisationRepository,
        ISuperSession superSession
    )
    {
        _userAuthorisationRepository = userAuthorisationRepository;
        _superSession = superSession;
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var user = _superSession.User;
        if (user is null)
        {
            context.Fail(new AuthorizationFailureReason(this, "No user logged in."));
            return Task.CompletedTask;
        }

        foreach(var requirement in context.PendingRequirements)
        {

            if (requirement.GetType() == typeof(RolesAuthorizationRequirement))
            {
                var permissions = user.Permissions.Select(s => s.ToString()).ToList();
                var rolesRequirement = (RolesAuthorizationRequirement)requirement;
                
                var hasPermission = rolesRequirement.AllowedRoles.Intersect(permissions).Any();
                if (hasPermission)
                {
                    context.Succeed(requirement);
                }
            }
        }

        return Task.CompletedTask;
    }
}
