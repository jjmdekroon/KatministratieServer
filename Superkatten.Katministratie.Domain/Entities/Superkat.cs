using Superkatten.Katministratie.Domain.Entities.Locations;
using Superkatten.Katministratie.Domain.Exceptions;
using System;

namespace Superkatten.Katministratie.Domain.Entities
{
    public class Superkat
    {
        public Guid Id { get; init; }
        public int Number { get; private set; }
        public SuperkatState State { get; private set; } = SuperkatState.New;
        public DateTime CatchDate { get; private set; } = DateTime.UtcNow;
        public CatchOrigin CatchOrigin { get; private set; }
        public bool Reserved { get; private set; } = false;
        public string Name { get; private set; } = string.Empty;
        public BaseLocation Location { get; private set; }

        public DateTime Birthday { get; private set; }
        public bool Retour { get; private set; }
        public CatBehaviour Behaviour { get; private set; } = CatBehaviour.Unknown;
        public AgeCategory AgeCategory { get; private set; }
        public Gender Gender { get; private set; } = Gender.Unknown;
        public LitterGranuleType LitterType { get; private set; } = LitterGranuleType.Normal;
        public bool WetFoodAllowed { get; private set; } = true;
        public FoodType FoodType { get; private set; } = FoodType.FirstPhase;
        public string Color { get; private set; } = string.Empty;
        public byte[]? Photo { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        // Constructor is needed for EF
        public Superkat() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Superkat(
            int number,
            DateTime catchDate,
            CatchOrigin catchOrigin,
            BaseLocation location
        )
        {
            Id = Guid.NewGuid();

            Number = number;
            CatchDate = catchDate;
            CatchOrigin = catchOrigin;
            Location = location;
        }

        public string UniqueNumber => CatchDate.Year.ToString() + "-" + Number.ToString("000");

        public Superkat CreateUpdatedModel(
            DateTime birthday,
            CatBehaviour catBehaviour,
            bool retour,
            AgeCategory ageCategory,
            Gender gender,
            LitterGranuleType litterType,
            bool wetFoodAllowed,
            FoodType foodType,
            string color
        )
        {
            return new Superkat(Number, CatchDate, CatchOrigin, Location)
            {
                Id = Id,

                Birthday = birthday,
                Behaviour = catBehaviour,
                Retour = retour,
                AgeCategory = ageCategory,
                Gender = gender,
                LitterType = litterType,
                WetFoodAllowed = wetFoodAllowed,
                FoodType = foodType,
                Color = color
            };
        }

        public void Relocate(BaseLocation newLocation)
        {
            Location = newLocation;
        }

        public void SetPhoto(byte[] photo)
        {
            Photo = photo;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetRetour(bool retour)
        {
            Retour = retour;
        }

        public void SetReserved(bool reserved)
        {
            Reserved = reserved;
        }

        public void StartAdoption()
        {
            State = SuperkatState.Adoption;
        }

        public void AbortAdoption()
        {
            State = SuperkatState.New;
        }

        public void FinishAdoption()
        {
            State = SuperkatState.Relocated;
        }
    }
}
