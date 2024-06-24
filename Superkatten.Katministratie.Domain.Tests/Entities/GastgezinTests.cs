using FluentAssertions;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Domain.Exceptions;
using Xunit;

namespace Superkatten.Katministratie.Domain.Tests.Entities
{
    public class GastgezinTests
    {
        [Fact]
        public void Gastgezin_WithEmptyName_ThrowsError()
        {
            // arrange

            // act
            Action act = () => new Gastgezin(string.Empty, null, null, null);

            // assert
            act.Should().ThrowExactly<DomainException>();
        }
    }
}
