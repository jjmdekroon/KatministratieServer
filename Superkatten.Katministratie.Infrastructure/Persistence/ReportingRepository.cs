using Microsoft.EntityFrameworkCore;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Domain.Entities.Locations;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Infrastructure.Persistence;

internal class ReportingRepository : IReportingRepository
{
    private readonly SuperkattenDbContext _context;

    public ReportingRepository(SuperkattenDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Superkat>> GetSuperkattenBetweenFromToAsync(DateTime from, DateTime to)
    {
        return await _context.SuperKatten
            .AsNoTracking()
            .Include(o => o.CatchOrigin)
            .Where(o => o.CatchDate >= from && o.CatchDate < to && o.State != SuperkatState.Adoption)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Superkat>> GetSuperkattenAtLocationAsync(CatArea catArea, int? cageNumber)
    {
        var superkatten = await _context.SuperKatten
            .AsNoTracking()
            .Include(o => o.CatchOrigin)
            .Where(o => o.State != SuperkatState.Adoption && IsLocation(o.Location, catArea, cageNumber))
            .ToListAsync();

        return superkatten ?? new List<Superkat>();
    }

    private static bool IsLocation(BaseLocation location, CatArea catArea, int? cageNumber)
    {
        if (location.LocationType is not LocationType.Refuge)
        {
            return false;
        }

        var refugeLocation = (Refuge)location;
        return refugeLocation.CatArea == catArea && refugeLocation.CageNumber == cageNumber;
    }

    public async Task<IReadOnlyCollection<Superkat>> GetNotNeutralizedSuperkatten()
    {
        var neutralizedSuperkatten = _context.MedicalProcedures
            .AsNoTracking()
            .Where(m => m.ProcedureType == MedicalProcedureType.Neutralize)
            .Select(m => m.SuperkatId)
            .ToList();

        return await _context.SuperKatten
            .AsNoTracking()
            .Include(o => o.CatchOrigin)
            .Include(o => o.Location)
            .Include(o => o.Location.LocationNaw)
            .Where(o => !neutralizedSuperkatten.Contains(o.Id) && o.State != SuperkatState.Adoption)
            .ToListAsync();
    }
}