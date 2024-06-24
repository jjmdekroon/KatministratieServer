using Microsoft.EntityFrameworkCore;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Infrastructure.Persistence;

public class CatchOriginRepository : ICatchOriginRepository
{
    private readonly SuperkattenDbContext _context;

    public CatchOriginRepository(SuperkattenDbContext context)
    {
        _context = context;
    }

    public async Task<CatchOrigin?> GetCatchOriginAsync(CatchOriginType type, string name)
    {
        return await _context
            .CatchOrigins
            .Where(l => l.Type == type && l.Name.ToLower().Equals(name.ToLower()))
            .FirstOrDefaultAsync();
    }

    public async Task<CatchOrigin> CreateCatchOriginAsync(CatchOriginType type, string name)
    {
        var location = new CatchOrigin(name, type);

        await _context.CatchOrigins.AddAsync(location);
        await _context.SaveChangesAsync();

        return location;
    }

    public async Task<IReadOnlyCollection<CatchOrigin>> GetCatchOriginsAsync()
    {
        return await _context
            .CatchOrigins
            .AsNoTracking()
            .ToListAsync();
    }
}
