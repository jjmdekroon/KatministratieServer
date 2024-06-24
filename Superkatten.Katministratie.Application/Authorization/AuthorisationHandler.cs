using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
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

    public AuthorisationHandler(IUserAuthorisationRepository userAuthorisationRepository)
    {
        _userAuthorisationRepository = userAuthorisationRepository;
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var userId = -1;
        try 
        { 
            _ = int.TryParse(context.User.Claims.First(c => c.Type == "id").Value, out userId); 
        } 
        catch (Exception) 
        { 
        };

        var user = _userAuthorisationRepository
            .GetAllUsers()
            .FirstOrDefault(u => u.Id == userId);

        if (user is null)
        {
            context.Fail(new AuthorizationFailureReason(this, "No user logged in."));
            return Task.CompletedTask;
        }

        foreach(var requirement in context.Requirements)
        {

            if (requirement.GetType() == typeof(RolesAuthorizationRequirement))
            {
                var permissions = user.Permissions.Select(s => s.ToString()).ToList();
                var rolesRequirement = (RolesAuthorizationRequirement)requirement;
                
                var hasPermission = rolesRequirement.AllowedRoles.Intersect(permissions).Any();
                if (hasPermission)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
        }

        var failException = new AuthorizationFailureReason(this, "User is not authorized to use this function");
        context.Fail(failException);
        return Task.CompletedTask;
    }
}
