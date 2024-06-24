using Superkatten.Katministratie.Application.Exceptions;
using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Domain.Entities.Locations;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<BaseLocation> CreateHostFamilyAsync(LocationNawParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.Name))
            {
                throw new ValidationException("Gastgezin name is invalid");
            }

            var gastgezin = new Gastgezin(
                parameters.Name,
                parameters.Postcode,
                parameters.Address,
                parameters.City,
                parameters.Phone,
                parameters.Email
                );

            await _locationRepository.CreateLocationAsync(gastgezin);

            return gastgezin;
        }
       
        public async Task DeleteLocationAsync(Guid id)
        {
            await _locationRepository.DeleteLocationAsync(id);
        }

        public async Task<IReadOnlyCollection<BaseLocation>> GetLocationsAsync()
        {
            var locations = await _locationRepository.GetLocationsAsync();
            
            return locations.ToList();
        }

        public async Task<BaseLocation> UpdateLocationAsync(Guid locationId, LocationNawParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.Name))
            {
                throw new ValidationException($"location name is empty");
            }

            var location = await _locationRepository.GetLocationAsync(locationId);
            if (location is null)
            {
                throw new ValidationException($"location with guid: {locationId} does not exsist");
            }

            location.UpdateNaw(
                parameters.Name,
                parameters.Address,
                parameters.Postcode,
                parameters.City,
                parameters.Phone,
                parameters.Email
            );

            await _locationRepository.UpdateLocationAsync(location);

            return location;
        }
    }
}
