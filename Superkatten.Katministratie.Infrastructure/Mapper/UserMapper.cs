using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Superkatten.Katministratie.Infrastructure.Mapper;

public class UserMapper : IUserMapper
{
    public UserDto MapDomainToRepository(User user)
    {
        return new UserDto
        {
            Email = user.Email,
            Id = user.Id,
            Name = user.Name,
            PasswordHash = user.PasswordHash,
            Username = user.Username,
            Permissions = MapPermissionsToString(user.Permissions)
        };
    }

    private static string MapPermissionsToString(IReadOnlyCollection<PermissionEnum> permissions)
    {
        var result = string.Join(",", permissions);
        return result ?? string.Empty;
    }

    public User MapRepositoryToDomain(UserDto userDto)
    {
        return new User
        {
            Email = userDto.Email,
            Id = userDto.Id,
            Name = userDto.Name,
            PasswordHash = userDto.PasswordHash,
            Username = userDto.Username,
            Permissions = MapToPermissions(userDto.Permissions)
        };
    }

    private IReadOnlyCollection<PermissionEnum> MapToPermissions(string permissions)
    {
        var permissionItems = permissions.Split(',');
        return permissionItems.Select(ConvertItemToEnum).ToList();
    }

    private static PermissionEnum ConvertItemToEnum(string permissionItem)
    {
        return Enum.Parse<PermissionEnum>(permissionItem, ignoreCase: true);
    }
}
