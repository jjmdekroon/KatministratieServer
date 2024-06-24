using Microsoft.Extensions.Logging;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Infrastructure.Entities;
using Superkatten.Katministratie.Infrastructure.Exceptions;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using Superkatten.Katministratie.Infrastructure.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Superkatten.Katministratie.Infrastructure.Persistence;

public class UserAuthorisationRepository : IUserAuthorisationRepository
{
    private readonly ILogger<SuperkattenRepository> _logger;
    private readonly SuperkattenDbContext _context;
    private readonly IUserMapper _userMapper;

    public UserAuthorisationRepository(
        ILogger<SuperkattenRepository> logger,
        SuperkattenDbContext context,
        IUserMapper userMapper
    )
    {
        _logger = logger;
        _context = context;
        _userMapper = userMapper;
    }

    public void DeleteUserById(int userId)
    {
        var user = GetUser(userId);
        
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    private UserDto GetUser(int id)
    {
        var user = GetAllUsers()
            .FirstOrDefault(u => u.Id == id);

        return user is null 
            ? throw new DatabaseException("User not found") 
            : _userMapper.MapDomainToRepository(user);
    }

    public IReadOnlyCollection<User> GetAllUsers()
    {
        return _context.Users
            .Select(_userMapper.MapRepositoryToDomain)
            .ToList();
    }

    public User? GetUserById(int userId)
    {
        var userDto = GetUser(userId);
        return _userMapper.MapRepositoryToDomain(userDto);
    }

    public User? GetUserByName(string userName)
    {
        var users = _context.Users.ToList();
        var userDto = _context.Users
            .SingleOrDefault(x => x.Username == userName);

        if (userDto is null)
        {
            throw new DatabaseException($"user with name '{userName}' is unknown in the database");
        }

        return _userMapper.MapRepositoryToDomain(userDto);
    }

    public void StoreUser(User user)
    {
        var userDto = _userMapper.MapDomainToRepository(user);
        _context.Users.Add(userDto);
        _context.SaveChanges();
    }

    public void UpdateUser(User updatedUser)
    {
        var userDto = GetUser(updatedUser.Id);
        if (userDto is null)
        {
            throw new Exception($"User id {updatedUser.Id} does not exsist");
        }

        userDto.Name = updatedUser.Name;
        userDto.Email = updatedUser.Email;
        userDto.Username = updatedUser.Username;
        
        _context.Users.Update(userDto);
        _context.SaveChanges();
    }
}
