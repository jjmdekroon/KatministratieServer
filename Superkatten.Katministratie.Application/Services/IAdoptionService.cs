using Superkatten.Katministratie.Contract.ApiInterface;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Services;

public interface IAdoptionService
{
    Task StartSuperkattenAdoptionAsync(StartAdoptionSuperkattenParameters reserveSuperkattenParameters);
}