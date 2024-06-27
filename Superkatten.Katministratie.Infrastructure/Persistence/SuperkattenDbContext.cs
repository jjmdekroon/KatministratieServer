using Microsoft.EntityFrameworkCore;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Domain.Entities.Locations;
using Superkatten.Katministratie.Infrastructure.Entities;
using System;

namespace Superkatten.Katministratie.Infrastructure.Persistence;

public class SuperkattenDbContext : DbContext
{
    public string DbPath { get; }

    public DbSet<UserDto> Users => Set<UserDto>();
    public DbSet<Superkat> SuperKatten => Set<Superkat>();
    public DbSet<CatchOrigin> CatchOrigins => Set<CatchOrigin>();
    public DbSet<LocationNaw> LocationNaw => Set<LocationNaw>();
    public DbSet<MedicalProcedure> MedicalProcedures => Set<MedicalProcedure>();
    public DbSet<BaseLocation> Locations => Set<BaseLocation>();

    public SuperkattenDbContext(DbContextOptions<SuperkattenDbContext> options) 
        : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join("", "katministratie.db");

        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gastgezin>();
        modelBuilder.Entity<Adoptant>();
        modelBuilder.Entity<Refuge>();

        modelBuilder.Entity<BaseLocation>()
            .HasOne(l => l.LocationNaw)
            .WithOne(naw => naw.Location)
            .HasForeignKey<LocationNaw>(fk => fk.Id);
    }
}
