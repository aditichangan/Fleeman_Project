using System.Collections.Generic;
using System.Threading.Tasks;
using FleetManagementSystem.Api.Models;

namespace FleetManagementSystem.Api.Services;

public interface ICarTypeMasterService
{
    Task<List<CarTypeMaster>> GetAllCarTypesAsync();
}
