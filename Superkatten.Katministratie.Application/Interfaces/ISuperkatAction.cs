using System;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Interfaces;

public interface ISuperkatAction
{
    Task ToggleRetourAsync(Guid id );
    Task ToggleReserveAsync(Guid id);
}
