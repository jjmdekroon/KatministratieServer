using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Contract.Entities.Locations;
using Superkatten.Katministratie.Host.Helpers;

namespace Superkatten.Katministratie.Host.Entities
{
    public class SuperkatView
    {
        public Superkat Superkat { get; set; }

        public string GenderIcon => $"./images/Gender/{Superkat.Gender}.png";

        public bool IsAtGastgezin => Superkat.Location.LocationType is LocationType.HostFamily;

        public SuperkatView(Superkat superkat)
        {
            if (superkat is null)
            {
                throw new ArgumentNullException(nameof(superkat));
            }

            Superkat = superkat;
        }

        public bool IsVisible { get; set; } = false;
    }
}
