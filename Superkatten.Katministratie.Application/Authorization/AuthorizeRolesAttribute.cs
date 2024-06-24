using Microsoft.AspNetCore.Authorization;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.Application.Authorization;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params PermissionEnum[] permissions) : base()
    {
        Roles = string.Join(",", permissions);
    }
}
