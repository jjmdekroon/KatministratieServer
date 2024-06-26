using Superkatten.Katministratie.Application.Authenticate;
using Superkatten.Katministratie.Application.Exceptions;
using Superkatten.Katministratie.Contract.Authenticate;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using BcryptNet = BCrypt.Net.BCrypt;

namespace Superkatten.Katministratie.Application.Services;

public class UserService : IUserService
{
    private readonly IJwtUtils _jwtUtils;
    private readonly IUserAuthorisationRepository _userAuthorisationRepository;
    private readonly IUserAuthorisationMapper _userAuthorisationMapper;
    private readonly ISuperSession _superSession;
    private User? _user;

    public UserService(
        IJwtUtils jwtUtils,
        IUserAuthorisationRepository userAuthorisationRepository,
        IUserAuthorisationMapper userAuthorisationMapper,
        ISuperSession superSession)
    {
        _jwtUtils = jwtUtils;
        _userAuthorisationRepository = userAuthorisationRepository;
        _userAuthorisationMapper = userAuthorisationMapper;
        _superSession = superSession;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        if (model.Username is null)
        {
            throw new AuthorisationException("Given username may not be null");
        }

        _user = _userAuthorisationRepository.GetUserByName(model.Username.ToLower());

        if (_user is null)
        {
            throw new AuthorisationException("Username is incorrect");
        }

        if (!BcryptNet.Verify(model.Password, _user.PasswordHash))
        {
            throw new AuthorisationException("Password is incorrect");
        }

        // authentication successfull, store all data within
        var token = _jwtUtils.GenerateToken(_user);
        
        _superSession?.StartWithUser(_user);

        return _userAuthorisationMapper.MapToAuthenticateResponse(_user, token);
    }

    public IEnumerable<User> GetAll()
    {
        return _userAuthorisationRepository.GetAllUsers();
    }

    public User? GetById(int id)
    {
        return _userAuthorisationRepository
            .GetAllUsers()
            .FirstOrDefault(u => u.Id == id);
    }

    public void Register(RegisterRequest model)
    {
        CheckForValidUsername(model.Username);

        var userExsist = _userAuthorisationRepository
            .GetAllUsers()
            .Any(x => x.Username == model.Username);

        if (userExsist)
        {
            throw new AuthorisationException("Username '" + model.Username + "' is already taken");
        }

        // hash password
        var passwordHash = BcryptNet.HashPassword(model.Password);

        // create domain user model
        var domainUser = new User
        {
            Name = model.Name,
            Email = model.Email,
            Username = model.Username,
            PasswordHash = passwordHash
        };

        // save user
        _userAuthorisationRepository.StoreUser(domainUser);
    }

    private static void CheckForValidUsername(string username)
    {
        var regex = new Regex(@"^[a-z]+$");
        var isValidUserName = regex.IsMatch(username);
        if (!isValidUserName)
        {
            throw new AuthorisationException("Username contains invallid characters, only [a...z] allowed.");
        }
    }

    public void Update(int id, UpdateRequest updateRequest)
    {
        var user = _userAuthorisationRepository
            .GetAllUsers()
            .FirstOrDefault(x => x.Id== id);

        if (user is null)
        {
            throw new AuthorisationException($"Username with id '{id}' is not found");
        }

        // validate
        var userExsist = _userAuthorisationRepository
            .GetAllUsers()
            .Any(x => x.Username == updateRequest.Username);

        if (updateRequest.Username != user.Username && userExsist)
        {
            throw new AuthorisationException("Username '" + updateRequest.Username + "' is already taken");
        }

        // hash password if it was entered
        var passwordHash = string.IsNullOrEmpty(updateRequest.Password)
            ? string.Empty
            :BcryptNet.HashPassword(updateRequest.Password);

        // copy model to user and save
        user = user.Update(updateRequest.Username, updateRequest.Email, updateRequest.Name, passwordHash);

        _userAuthorisationRepository.UpdateUser(user);
    }

    public void Delete(int id)
    {
        _userAuthorisationRepository.DeleteUserById(id);
    }
}
