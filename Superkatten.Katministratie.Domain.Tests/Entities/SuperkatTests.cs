using FluentAssertions;
using Moq;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Domain.Exceptions;
using Xunit;

namespace Superkatten.Katministratie.Domain.Tests.Entities
{
    //TODO: isAny eruit, gebruik SuperkatBuilder
    public class SuperkatTests
    {
        [Fact]
        public void CreateUpdatedModel_WithName_Success()
        {
            // arrange
            const string KAT_NAME = "katnaam";
            var sut = new Superkat( //TODO: remove the It.IsAny -> replace by created type
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<string>(),
                It.IsAny<bool>()
            );

            // act
            var superkat = sut.SetName(KAT_NAME);

            // assert
            superkat.Name.Should().Be(KAT_NAME);
        }

        [Fact]
        public void CreateUpdatedModel_WithNewBirthday_Success()
        {
            // arrange
            var foundDate = DateTimeOffset.Now;
            var sut = new Superkat(
                It.IsAny<int>(),
                foundDate,
                It.IsAny<string>(),
                It.IsAny<bool>()
            );

            var birthday = foundDate.AddDays(-10);

            // act
            var superkat = sut.SetBirthday(birthday);

            // assert
            superkat.Birthday.Should().Be(birthday);
        }

        [Fact]
        public void CreateUpdatedModel_WithBirthdayAfterFoundDate_ThrowsException()
        {
            // arrange
            var foundDate = DateTimeOffset.Now;
            var sut = new Superkat(
                It.IsAny<int>(),
                foundDate,
                It.IsAny<string>(),
                It.IsAny<bool>()
            );

            var birthday = foundDate.AddDays(10);

            // act
            var act = () => sut.SetBirthday(birthday);

            // assert
            act.Should()
                .ThrowExactly<DomainException>()
                .WithMessage("Birthday '*' is larger or equal than founddate '*'");
        }
    }
}