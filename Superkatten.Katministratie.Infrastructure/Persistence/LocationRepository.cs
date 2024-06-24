using Microsoft.EntityFrameworkCore;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Domain.Entities.Locations;
using Superkatten.Katministratie.Infrastructure.Exceptions;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Infrastructure.Persistence;

public class LocationRepository : ILocationRepository
{
    private readonly SuperkattenDbContext _context;

    public LocationRepository(SuperkattenDbContext context)
    {
        _context = context;
    }

    public async Task CreateLocationAsync(BaseLocation location)
    {
        var locationExists = await _context
            .Locations
            .AnyAsync(o => o.LocationNaw.Name == location.LocationNaw.Name);

        if (locationExists)
        {
            throw new DatabaseException($"A hostfamily found in the database with name '{location.LocationNaw.Name}'");
        }

        await _context.Locations.AddAsync(location);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLocationAsync(Guid id)
    {
        var location = await _context
            .Locations
            .AsNoTracking()
            .Where(o => o.Id == id)
            .FirstAsync();

        if (location is null)
        {
            throw new DatabaseException($"No hostfamily found in the database with id '{id}'");
        }

        _context.Locations.Remove(location);
        _context.SaveChanges();
    }

    public async Task<IReadOnlyCollection<BaseLocation>> GetLocationsAsync()
    {
        return await _context
            .Locations
            .Include(o => o.LocationNaw)
            .ToListAsync();
    }

    public async Task<LocationNaw?> GetLocationNawAsync(string name)
    {
        var locationNaw = await _context
            .LocationNaw
            .FirstOrDefaultAsync(o => o.Name.Equals(name));

        return locationNaw;
    }

    public async Task<BaseLocation> GetLocationAsync(Guid locationId)
    { 
        var location = await _context
            .Locations
            .Include(o => o.LocationNaw)
            .AsNoTracking()
            .Where(o => o.Id == locationId)
            .FirstOrDefaultAsync();

        return location is not null
            ? location
            : throw new DatabaseException($"Location not found with id '{locationId}'");
    }

    public async Task UpdateLocationAsync(BaseLocation location)
    {
        var locationExsist = await _context
            .Locations
            .AnyAsync(o => o.Id == location.Id);
        if (!locationExsist)
        {
            throw new DatabaseException($"Location with name '{location.LocationNaw.Name}' not found");
        }

        _context.Locations.Update(location);
        _ = await _context.SaveChangesAsync();
    }
}
