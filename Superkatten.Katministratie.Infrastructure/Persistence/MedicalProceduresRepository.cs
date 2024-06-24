using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Infrastructure.Persistence;

public class MedicalProceduresRepository : IMedicalProceduresRepository
{
    private readonly ILogger<MedicalProceduresRepository> _logger;
    private readonly SuperkattenDbContext _context;

    public MedicalProceduresRepository(
        ILogger<MedicalProceduresRepository> logger,
        SuperkattenDbContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    public async Task AddMedicalProcedureAsync(MedicalProcedure medicalProcedure)
    {
        await _context.MedicalProcedures.AddAsync(medicalProcedure);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<MedicalProcedure>> GetAllMedicalProcedureAsync()
    {
        return await _context
            .MedicalProcedures
            .AsNoTracking()
            .ToListAsync();
    }
}
