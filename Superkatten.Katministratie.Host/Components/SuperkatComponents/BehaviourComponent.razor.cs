using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Host.Components.SuperkatComponents;

public partial class BehaviourComponent : ComponentBase
{
    [Parameter]
    public CatBehaviour Behaviour { get; set; }

    public string TooltipText
    {
        get
        {
            return Behaviour switch
            {
                CatBehaviour.Unknown => "Pas op gedrag niet bekend",
                CatBehaviour.Shy => "Pas op dit is een schuwe kat",
                CatBehaviour.Social => "Deze kan je aaien",
                _ => "Pas op gedrag niet bekend",
            };
        }
    }

    public string BehaviourIconName
    {
        get
        {
            return Behaviour switch
            {
                CatBehaviour.Unknown => "./Images/Behaviour/Onbekend.jpg",
                CatBehaviour.Shy => "./Images/Behaviour/Schuw.jpg",
                CatBehaviour.Social => "./Images/Behaviour/Sociaal.jpg",
                _ => "./Images/Behaviour/Error.jpg",
            };
        }
    }
}
