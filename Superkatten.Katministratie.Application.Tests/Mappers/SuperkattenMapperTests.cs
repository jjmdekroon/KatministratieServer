using FluentAssertions;
using Moq;
using Superkatten.Katministratie.Application.Mappers;
using Superkatten.Katministratie.Domain.Entities;
using Xunit;

namespace Superkatten.Katministratie.Application.Tests.Mappers
{
    public class SuperkattenMapperTests
    {
        private readonly string SUPERKAT_COLOR = "red";
        private readonly string SUPERKAT_CATCH_LOCATION = "rhenoy";
        private readonly int SUPERKAT_NUMBER = 1;
        [Fact]
        public void SuperkattenMapper_MapToDomain_Success()
        {
            //arrange
            var foundDate = DateTime.Now;
            var sut = new SuperkattenMapper();
            var contractSuperkat = new Contracts.Superkat()
            {
                Number = SUPERKAT_NUMBER,
                SuperkatColor = SUPERKAT_COLOR,
                FoundDate = foundDate,
                CatchLocation = SUPERKAT_CATCH_LOCATION,
                Birthday = foundDate.AddDays(-1),
                Name = string.Empty
            };

            var expectedSuperkatInstance = new Superkat(
                    number: SUPERKAT_NUMBER,
                    foundDate: foundDate,
                    catchLocation: SUPERKAT_CATCH_LOCATION,
                    isGoingRetour: false
                );
            expectedSuperkatInstance.SetBirthday(foundDate.AddDays(-1));

            // act
            var result = sut.MapToDomain(contractSuperkat);

            // assert
            result
                .Should()
                .BeEquivalentTo(expectedSuperkatInstance);
        }

        [Fact]
        public void SupperkattenMapper_MapFromDomain_Success()
        {
            // arrange
            var foundDate = DateTime.Now;
            var sut = new SuperkattenMapper();
            var superkat = new Superkat(
                SUPERKAT_NUMBER,
                foundDate,
                SUPERKAT_CATCH_LOCATION,
                false
            );
            superkat.SetBirthday(foundDate.AddDays(-1));

            var expectedContractSuperkat = new Contracts.Superkat
            {
                Number = SUPERKAT_NUMBER,
                Name = string.Empty,
                FoundDate = foundDate,
                CatchLocation = SUPERKAT_CATCH_LOCATION,
                SuperkatColor = SUPERKAT_COLOR,
                Birthday = foundDate.AddDays(-1)
            };

            // act
            var contractSuperkat = sut.MapFromDomain(superkat);

            // assert
            contractSuperkat
                .Should()
                .BeEquivalentTo(expectedContractSuperkat);
        }

    }
}
