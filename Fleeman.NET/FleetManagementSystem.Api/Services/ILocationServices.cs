using System.Collections.Generic;
using FleetManagementSystem.Api.Models;

namespace FleetManagementSystem.Api.Services;

public interface IStateService
{
    Task<List<StateMaster>> GetAllStatesAsync();
}

public interface ICityService
{
    Task<List<CityMaster>> GetAllCitiesAsync();
    Task<List<CityMaster>> GetCitiesByStateIdAsync(int stateId);
}

public interface IHubService
{
    Task<List<HubMaster>> GetAllHubsAsync();
    Task<List<HubMaster>> GetHubsByCityIdAsync(int cityId);
    Task<HubMaster> GetHubByIdAsync(int hubId);
    Task<List<HubMaster>> SearchHubsByAirportCodeAsync(string airportCode);
}

public interface IAirportService
{
    Task<List<AirportMaster>> GetAllAirportsAsync();
    Task<List<AirportMaster>> GetAirportsByStateIdAsync(int stateId);
}
