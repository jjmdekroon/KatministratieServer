using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Infrastructure.Exceptions;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Infrastructure.Persistence;

public class SuperkattenRepository : ISuperkattenRepository
{
    private readonly ILogger<SuperkattenRepository> _logger;
    private readonly SuperkattenDbContext _context;

    public SuperkattenRepository(
        ILogger<SuperkattenRepository> logger, 
        SuperkattenDbContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    public async Task CreateSuperkatAsync(Superkat superkat)
    {
        var superkatDtoExsist = await _context
            .SuperKatten
            .AsNoTracking()
            .AnyAsync(s => s.Id == superkat.Id);
        if (superkatDtoExsist)
        {
            throw new DatabaseException($"A {nameof(Superkat)} found in the database with id {superkat.Id}");
        }

        await _context.SuperKatten.AddAsync(superkat);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSuperkatAsync(Guid guid)
    {
        var superkat = await _context
            .SuperKatten
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == guid);
        if (superkat is null)
        {
            throw new DatabaseException($"No superkat found in the database with id {guid}");
        }

        _context.SuperKatten.Remove(superkat);
        _context.SaveChanges();
    }

    public async Task<IReadOnlyCollection<Superkat>> GetSuperkattenAsync()
    {
        var superkatten = await _context
            .SuperKatten
            .Where(o => o.State != SuperkatState.Adoption)
            .Include(o => o.CatchOrigin)
            .Include(o => o.Location)
            .Include(o => o.Location.LocationNaw)
            .AsNoTracking()
            .ToListAsync();

        return superkatten;
    }

    public async Task<Superkat> GetSuperkatAsync(Guid id)
    {
        var superkat = await _context
            .SuperKatten
            .Include(o => o.CatchOrigin)
            .Include(o => o.Location)
            .Include(o => o.Location.LocationNaw)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        return superkat is null 
            ? throw new DatabaseException($"No superkat found in the database with id {id}") 
            : superkat;
    }

    public async Task UpdateSuperkatAsync(Superkat superkat)
    {
        // Check naar applicatielaag?
        var superkatExist = await _context
            .SuperKatten
            .Include(o => o.CatchOrigin)
            .Include(o => o.Location)
            .Include(o => o.Location.LocationNaw)
            .AnyAsync(s => s.Id == superkat.Id);
        if (!superkatExist)
        {
            throw new DatabaseException($"No superkat found in the database with id {superkat.Id}");
        }

        var locationExists = await _context
            .Locations
            .Include(o => o.LocationNaw)
            .AsNoTracking()
            .AnyAsync(l => l.Id == superkat.Location.Id);
        if (!locationExists)
        {
            _context.Locations.Add(superkat.Location);
            await _context.SaveChangesAsync();
        }

        try
        {
            _context.Update(superkat);
        }
        catch (Exception ex)
        {

        }
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetMaxSuperkatNumberForYear(int year)
    {
        var highestOrderNumberForYear = await _context
            .SuperKatten
            .Where(s => s.CatchDate.Year == year)
            .MaxAsync(s => (int?)s.Number);

        return highestOrderNumberForYear ?? 0;
    }
}
