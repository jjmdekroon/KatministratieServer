using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Infrastructure.Entities;

namespace Superkatten.Katministratie.Infrastructure.Mapper;

public interface IUserMapper
{
    UserDto MapDomainToRepository(User user);
    User MapRepositoryToDomain(UserDto userDto);
}
