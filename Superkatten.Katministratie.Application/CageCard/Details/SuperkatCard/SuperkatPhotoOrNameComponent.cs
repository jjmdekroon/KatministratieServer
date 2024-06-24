using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.Application.CageCard.Details.SuperkatCard;

public class SuperkatPhotoOrNameComponent : IComponent
{
    private readonly Superkat _superkat;

    public SuperkatPhotoOrNameComponent(Superkat superkat)
    {
        _superkat = superkat;
    }

    public void Compose(IContainer container)
    {
        container.Column(column => 
        {
            if (_superkat.Photo is null)
            {
                column.Item()
                    .Padding(2)
                    .AlignCenter()
                    .Text(_superkat.Name);

                return;
            }
        
            column.Item()
                .Image(_superkat.Photo);
        });
    }
}
