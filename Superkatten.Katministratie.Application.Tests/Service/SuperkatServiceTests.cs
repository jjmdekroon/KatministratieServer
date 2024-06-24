using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Superkatten.Katministratie.Application.Services;
using Moq;
using Microsoft.Extensions.Logging;
using Superkatten.Katministratie.Domain.Interfaces;
using Superkatten.Katministratie.Application.Mappers;
using Superkatten.Katministratie.Application.Contracts;
using FluentAssertions;
using Superkatten.Katministratie.Application.Exceptions;

namespace Superkatten.Katministratie.Application.Tests.Service
{
    public class SuperkatServiceTests
    {
        private const int SUPERKATTEN_COUNT = 10;
        private const string SUPERKAT_CATCH_LOCATION = "rhenoy";
        private const string SUPERKAT_COLOR = "red";

        [Fact]
        public async void SuperkatService_CreateSuperkat_HasValidNumber()
        {
            // arrange
            var superkattenRepository = new Mock<ISuperkattenRepository>();
            superkattenRepository
                .Setup(s => s.GetSuperkatMaxNumberForGivenYearAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(SUPERKATTEN_COUNT));

            var superkattenMapper = new SuperkattenMapper();

            var superkattenService = new SuperkattenService(Mock.Of<ILogger<SuperkattenService>>(), superkattenRepository.Object, superkattenMapper);

            var createSuperkatParameters = new CreateSuperkatParameters
            {
                CatchLocation = SUPERKAT_CATCH_LOCATION,
                HasStronghold = true,
                WeeksOld = 21
            };

            // act
            var superkat = await superkattenService.CreateSuperkatAsync(createSuperkatParameters);

            //assert
            superkat.Number.Should().Be(SUPERKATTEN_COUNT + 1);
        }

        [Fact]
        public async void SuperkatService_CreateSuperkatWithEmptyName_ThrowsException()
        {
            // arrange
            var superkattenRepository = new Mock<ISuperkattenRepository>();
            superkattenRepository
                .Setup(s => s.GetSuperkatMaxNumberForGivenYearAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(SUPERKATTEN_COUNT));

            var superkattenMapper = new SuperkattenMapper();
            var superkattenService = new SuperkattenService(
                Mock.Of<ILogger<SuperkattenService>>(), 
                superkattenRepository.Object, 
                superkattenMapper
                );

            var createSuperkatParameters = new CreateSuperkatParameters
            {
                CatchLocation = SUPERKAT_CATCH_LOCATION,
                WeeksOld = 1,
                HasStronghold = true
            };

            // act
            var act = () => superkattenService.CreateSuperkatAsync(createSuperkatParameters);

            //assert
            await act
                .Should()
                .ThrowExactlyAsync<ValidationException>()
                .WithMessage("Superkat kleur is invalid");
        }

        [Fact]
        public async void SuperkatService_CreateSuperkatWithoutLocation_ThrowsException()
        {
            // arrange
            var superkattenRepository = new Mock<ISuperkattenRepository>();
            superkattenRepository
                .Setup(s => s.GetSuperkatMaxNumberForGivenYearAsync(DateTime.Now.Year))
                .Returns(Task.FromResult(SUPERKATTEN_COUNT));

            var superkattenMapper = new SuperkattenMapper();

            var superkattenService = new SuperkattenService(Mock.Of<ILogger<SuperkattenService>>(), superkattenRepository.Object, superkattenMapper);

            var createSuperkatParameters = new CreateSuperkatParameters
            {

                CatchLocation = string.Empty,
                WeeksOld = 1,
                HasStronghold = true
            };

            // act
            var act = () => superkattenService.CreateSuperkatAsync(createSuperkatParameters);

            //assert
            await act
                .Should()
                .ThrowExactlyAsync<ValidationException>()
                .WithMessage("Superkat location is empty");
        }

        [Fact]
        public async void SuperkatService_CreateSuperkatWithInvallidColor_ThrowsException()
        {
            // arrange
            var superkattenRepository = new Mock<ISuperkattenRepository>();
            superkattenRepository
                .Setup(s => s.GetSuperkatMaxNumberForGivenYearAsync(DateTime.Now.Year))
                .Returns(Task.FromResult(SUPERKATTEN_COUNT));

            var superkattenMapper = new SuperkattenMapper();

            var superkattenService = new SuperkattenService(Mock.Of<ILogger<SuperkattenService>>(), superkattenRepository.Object, superkattenMapper);

            var createSuperkatParameters = new CreateSuperkatParameters
            {

                CatchLocation = SUPERKAT_CATCH_LOCATION,
                WeeksOld = 1,
                HasStronghold = true
            };

            // act
            var act = () => superkattenService.CreateSuperkatAsync(createSuperkatParameters);

            //assert
            await act
                .Should()
                .ThrowExactlyAsync<ValidationException>()
                .WithMessage("Superkat kleur is invalid");
        }
    }
}
