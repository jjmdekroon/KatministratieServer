using FluentAssertions;
using Moq;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Infrastructure.Entities;
using Superkatten.Katministratie.Infrastructure.Mapper;
using Xunit;

namespace Superkatten.Katministratie.Infrastructure.Tests.Mapper
{
    public class SuperkatRepositoryMapperTests
    {
        private readonly string SUPERKAT_NAME = "katnaam";
        private readonly string SUPERKAT_COLOR = "red";
        private readonly string SUPERKAT_CATCH_LOCATION = "rhenoy";
        private readonly int SUPERKAT_NUMBER = 1;
        [Fact]
        public void SuperkatRepositoryMapper_MapDomainToSuperkatDto_Success()
        {
            // arrange
            var sut = new SuperkatRepositoryMapper();

            var today = DateTimeOffset.Now;
            var birthday = today.AddDays(-1);
            var superkat = new Superkat(
                SUPERKAT_NUMBER,
                today,
                SUPERKAT_CATCH_LOCATION,
                false
            ).SetName(SUPERKAT_NAME)
             .SetBirthday(birthday)
             .SetColor(SUPERKAT_COLOR);
            

            // act
            var superkatDto = sut.MapDomainToSuperkatDto(superkat);

            // assert
            superkatDto
                .Should()
                .BeEquivalentTo(
                new SuperkatDto
                {
                    Id = 0,
                    Number = SUPERKAT_NUMBER,
                    Name = SUPERKAT_NAME,
                    CatchLocation = SUPERKAT_CATCH_LOCATION,
                    FoundDate = today,
                    SuperkatColor = SUPERKAT_COLOR,
                    Birthday = birthday,
                    IsGoingRetour = true
                });

        }

        [Fact]
        public void SuperkatRepositoryMapperTests_MapSuperkatDtoToDomain_Success()
        {
            // arrange
            var sut = new SuperkatRepositoryMapper();
            var foundDate = DateTimeOffset.Now;
            var birthday = foundDate.AddDays(-1);
            var superkatDto = new SuperkatDto
            {
                Id = It.IsAny<int>(),
                Number = SUPERKAT_NUMBER,
                Name = SUPERKAT_NAME,
                CatchLocation = SUPERKAT_CATCH_LOCATION,
                FoundDate = foundDate,
                SuperkatColor = SUPERKAT_COLOR,
                Birthday = birthday,
                IsGoingRetour = false
            };

            // act
            var superkat = sut.MapSuperkatDtoToDomain(superkatDto);

            // assert
            superkat
                .Should()
                .BeEquivalentTo(
                    new Superkat(
                        SUPERKAT_NUMBER,
                        foundDate,
                        SUPERKAT_CATCH_LOCATION,
                        false
                    )
                    .SetName(SUPERKAT_NAME)
                    .SetBirthday(birthday)
                    .SetColor(SUPERKAT_COLOR)
                );
        }
    }
}
