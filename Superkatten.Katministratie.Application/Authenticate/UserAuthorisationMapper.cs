using Superkatten.Katministratie.Contract.Authenticate;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.Application.Authenticate
{
    //TODO: Deze mapper moet nog goed worden bekeken hoe deze met name de permissions zet bij registratie en update

    public class UserAuthorisationMapper : IUserAuthorisationMapper
    {
        public User MapToDomain(int id, UpdateRequest model, string? passwordHash)
        {
            var user = new User
            {
                Id = id,
                Name = model.Name,
                Email = model.Email,
                Username = model.Username,
                PasswordHash = passwordHash
            };
            return user;
        }

        public AuthenticateResponse MapToAuthenticateResponse(User user, string token)
        {
            var authenticateResponse = new AuthenticateResponse
            {
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                Id = user.Id,
                Token = token
            };
            return authenticateResponse;
        }
    }
}
