using Superkatten.Katministratie.Application.Exceptions;
using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Application.Mappers;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.ApiInterface.Reallocate;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Domain.Entities.Locations;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using Superkatten.Katministratie.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Services
{
    public class SuperkattenService : ISuperkattenService
    {
        public const int DAY_IN_ONE_WEEK = 7;

        private readonly ISuperkattenRepository _superkattenRepository;
        private readonly IReportingRepository _reportingRepository;
        private readonly IMedicalProceduresRepository _medicalProceduresRepository;
        private readonly ISuperkatMapper _superkattenMapper;
        private readonly ICatchOriginRepository _catchOriginRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationMapper _locationMapper;

        public SuperkattenService(
            ISuperkattenRepository superkattenRepository,
            ILocationRepository locationRepository,
            IReportingRepository reportingRepository,
            IMedicalProceduresRepository medicalProceduresRepository,
            ICatchOriginRepository catchOriginRepository,
            ISuperkatMapper superkattenMapper,
            ILocationMapper locationMapper)
        {
            _superkattenRepository = superkattenRepository;
            _reportingRepository = reportingRepository;
            _medicalProceduresRepository = medicalProceduresRepository;
            _superkattenMapper = superkattenMapper;
            _catchOriginRepository = catchOriginRepository;
            _locationRepository = locationRepository;
            _locationMapper = locationMapper;
        }

        public async Task<Superkat> CreateSuperkatAsync(CreateSuperkatParameters createSuperkatParameters)
        {                        
            var maxSuperkatNumberForYear = await _superkattenRepository.GetMaxSuperkatNumberForYear(DateTimeOffset.Now.Year);
            var catchOrigin = await GetOrCreateCatchOriginFromParametersAsync(createSuperkatParameters);
            var location = await GetOrCreateLocationFromParametersAsync(createSuperkatParameters);

            var superkat = new Superkat(
                maxSuperkatNumberForYear + 1, 
                createSuperkatParameters.CatchDate,
                catchOrigin,
                location
            );
            
            superkat = UpdateSuperkatDetails(superkat, createSuperkatParameters);

            await _superkattenRepository.CreateSuperkatAsync(superkat);

            if (createSuperkatParameters.StrongholdGiven)
            {
                var medicalProcedure = new MedicalProcedure(
                    MedicalProcedureType.Stronghold,
                    superkat.Id,
                    DateTime.UtcNow,
                    "tijdens vangen");
                await _medicalProceduresRepository.AddMedicalProcedureAsync(medicalProcedure);
            }            

            return superkat;
        }

        private async Task<BaseLocation> GetOrCreateLocationFromParametersAsync(CreateSuperkatParameters createSuperkatParameters)
        {
            var catArea = _superkattenMapper.MapContractToDomain(createSuperkatParameters.CatArea);
            var locations = await _locationRepository.GetLocationsAsync();
            var location = locations
                .Where(location => location.LocationType == LocationType.Refuge)
                .Select(location => (Refuge)location)
                .Where(refuge => refuge.CatArea == catArea && refuge.CageNumber == createSuperkatParameters.CageNumber)
                .FirstOrDefault();

            return location is not null 
                ? location 
                : new Refuge(catArea, createSuperkatParameters.CageNumber);
        }

        private async Task<CatchOrigin> GetOrCreateCatchOriginFromParametersAsync(CreateSuperkatParameters createSuperkatParameters)
        {
            var catchOriginDetails = _superkattenMapper.MapContractToDomain(createSuperkatParameters.CatchOrigin);
            var catchOrigin = await _catchOriginRepository.GetCatchOriginAsync(catchOriginDetails.Type, catchOriginDetails.Name);
            if (catchOrigin is null)
            {
                return new CatchOrigin(catchOriginDetails.Name, catchOriginDetails.Type);
            }

            return catchOrigin;
        }

        private Superkat UpdateSuperkatDetails(Superkat superkat, CreateSuperkatParameters createSuperkatParameters)
        {
            var catchDate = createSuperkatParameters.CatchDate;
            var estimatedBirthday = catchDate.AddDays(-DAY_IN_ONE_WEEK * createSuperkatParameters.EstimatedWeeksOld);

            var newSuperkat = superkat.CreateUpdatedModel(
                estimatedBirthday,
                _superkattenMapper.MapContractToDomain(createSuperkatParameters.Behaviour),
                createSuperkatParameters.Retour,
                _superkattenMapper.MapContractToDomain(createSuperkatParameters.AgeCategory),
                _superkattenMapper.MapContractToDomain(createSuperkatParameters.Gender),
                _superkattenMapper.MapContractToDomain(createSuperkatParameters.LitterType),
                createSuperkatParameters.WetFoodAllowed,
                _superkattenMapper.MapContractToDomain(createSuperkatParameters.FoodType),
                createSuperkatParameters.CatColor);

            return newSuperkat;
        }

        public async Task DeleteSuperkatAsync(Guid guid)
        {
            await _superkattenRepository.DeleteSuperkatAsync(guid);
        }

        public async Task<IReadOnlyCollection<Superkat>> ReadAllSuperkattenAsync()
        {
            return await _superkattenRepository.GetSuperkattenAsync();
        }

        public async Task<IReadOnlyCollection<Superkat>> ReadAvailableSuperkattenAsync()
        {
            var superkatten = await _superkattenRepository.GetSuperkattenAsync();

            return superkatten
                .Where(s => !s.Retour && !s.Reserved && s.Location.LocationType == LocationType.Refuge)
                .ToList();
        }

        public async Task<Superkat> ReadSuperkatAsync(Guid guid)
        {
            var superkat = await _superkattenRepository.GetSuperkatAsync(guid);
            return superkat;
        }

        public async Task<Superkat> UpdateSuperkatAsync(Guid guid, UpdateSuperkatParameters updateSuperkatParameters)
        {
            var superkat = await _superkattenRepository.GetSuperkatAsync(guid);
            if (superkat is null)
            {
                throw new ServiceException($"Superkat cannot be found for id {guid}");
            }

            superkat.SetName(updateSuperkatParameters.Name);

            await _superkattenRepository.UpdateSuperkatAsync(superkat);

            return superkat;
        }

        public async Task<Superkat> ReallocateToGastgezinAsync(Guid superkatId, ReallocateToGastgezinParameters parameters)
        {
            var superkat = await _superkattenRepository.GetSuperkatAsync(superkatId);
            if (superkat is null)
            {
                throw new ServiceException($"Superkat cannot be found for id {superkatId}");
            }

            var location = await _locationRepository.GetLocationAsync(parameters.LocationId);
            if (location is null)
            {
                throw new ServiceException($"Hostfamily cannot be found for id {parameters.LocationId}");
            }

            superkat.Relocate(location);

            await _superkattenRepository.UpdateSuperkatAsync(superkat);

            return superkat;
        }

        public async Task<Superkat> ReallocateInRefugeAsync(ReallocateInRefugeParameters parameters)
        {
            var superkat = await _superkattenRepository.GetSuperkatAsync(parameters.SuperkatId);
            if (superkat is null)
            {
                throw new ServiceException($"Superkat cannot be found for id {parameters.SuperkatId}");
            }

            // Get the new location
            var catArea = _superkattenMapper.MapContractToDomain(parameters.CatArea);
            var locations = await _locationRepository.GetLocationsAsync();
            var refugeLocation = locations
                .Where(location => location.LocationType == LocationType.Refuge)
                .Select(location => (Refuge)location)
                .FirstOrDefault(refugeLocation => refugeLocation.CageNumber == parameters.CageNumber
                                      && refugeLocation.CatArea == catArea);

            if (refugeLocation is null)
            {
                refugeLocation = new Refuge(catArea, parameters.CageNumber);
            }

            // Relocate
            superkat.Relocate(refugeLocation);

            // Update database
            await _superkattenRepository.UpdateSuperkatAsync(superkat);

            return superkat;
        }

        public async Task<Superkat> UpdateSuperkatAsync(Guid guid, PhotoParameters parameters)
        {
            var superkat = await _superkattenRepository.GetSuperkatAsync(guid);
            if (superkat is null)
            {
                throw new ServiceException($"Superkat cannot be found for id {guid}");
            }

            superkat.SetPhoto(parameters.Photo ?? Array.Empty<byte>());

            await _superkattenRepository.UpdateSuperkatAsync(superkat);

            return superkat;
        }


        public Task<IReadOnlyCollection<Superkat>> ReadNotNeutralizedSuperkattenAsync()
        {
            return _reportingRepository.GetNotNeutralizedSuperkatten();
        }
    }
}