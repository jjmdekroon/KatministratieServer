using Superkatten.Katministratie.Contract.Authenticate;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.Application.Authenticate;

public interface IUserAuthorisationMapper
{
    AuthenticateResponse MapToAuthenticateResponse(User user, string token);
    User MapToDomain(int id, UpdateRequest model, string? passwordHash);
}
