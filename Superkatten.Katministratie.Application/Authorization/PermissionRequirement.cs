using Microsoft.AspNetCore.Authorization;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.Application.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(PermissionEnum permission)
    {
        Permission = permission;
    }

    public PermissionEnum Permission { get; }
}
