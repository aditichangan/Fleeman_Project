using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FleetManagementSystem.Api.Models;
using FleetManagementSystem.Api.Services;
using Microsoft.Extensions.Logging;

namespace FleetManagementSystem.Api.Controllers;

[ApiController]
[Route("api/v1")]
public class LocationController : ControllerBase
{
    private readonly IHubService _hubService;
    private readonly ILogger<LocationController> _logger;
    public LocationController(IHubService hubService, ILogger<LocationController> logger)
    {
        _hubService = hubService;
        _logger = logger;
    }

    [HttpGet("locations/search")]
    public async Task<ActionResult<List<HubMaster>>> SearchLocations([FromQuery] string query)
    {
        _logger.LogInformation("Location search query: {Query}", query);
        return Ok(await _hubService.SearchHubsByAirportCodeAsync(query));
    }
}

[ApiController]
[Route("api/v1")]
public class StateController : ControllerBase
{
    private readonly IStateService _stateService;
    private readonly ILogger<StateController> _logger;
    public StateController(IStateService stateService, ILogger<StateController> logger)
    {
        _stateService = stateService;
        _logger = logger;
    }

    [HttpGet("states")]
    public async Task<ActionResult<List<StateMaster>>> GetAllStates()
    {
        _logger.LogInformation("Fetching all states");
        return Ok(await _stateService.GetAllStatesAsync());
    }
}

[ApiController]
[Route("api/v1")]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly ILogger<CityController> _logger;
    public CityController(ICityService cityService, ILogger<CityController> logger)
    {
        _cityService = cityService;
        _logger = logger;
    }

    [HttpGet("cities")]
    public async Task<ActionResult<List<CityMaster>>> GetAllCities()
    {
        _logger.LogInformation("Fetching all cities");
        return Ok(await _cityService.GetAllCitiesAsync());
    }

    [HttpGet("cities/state/{stateId}")]
    public async Task<ActionResult<List<CityMaster>>> GetCitiesByStateId(int stateId)
    {
        _logger.LogInformation("Fetching cities for StateId: {StateId}", stateId);
        return Ok(await _cityService.GetCitiesByStateIdAsync(stateId));
    }
}

[ApiController]
[Route("api/v1")]
public class HubController : ControllerBase
{
    private readonly IHubService _hubService;
    private readonly ILogger<HubController> _logger;
    public HubController(IHubService hubService, ILogger<HubController> logger)
    {
        _hubService = hubService;
        _logger = logger;
    }

    [HttpGet("hubs")]
    public async Task<ActionResult<List<HubMaster>>> GetAllHubs()
    {
        _logger.LogInformation("Fetching all hubs");
        return Ok(await _hubService.GetAllHubsAsync());
    }
    
    [HttpGet("hubs/city/{cityId}")]
    public async Task<ActionResult<List<HubMaster>>> GetHubsByCityId(int cityId)
    {
        _logger.LogInformation("Fetching hubs for CityId: {CityId}", cityId);
        return Ok(await _hubService.GetHubsByCityIdAsync(cityId));
    }
}

[ApiController]
[Route("api/v1")]
public class AirportController : ControllerBase
{
    private readonly IAirportService _airportService;
    private readonly ILogger<AirportController> _logger;
    public AirportController(IAirportService airportService, ILogger<AirportController> logger)
    {
        _airportService = airportService;
        _logger = logger;
    }

    [HttpGet("airports")]
    public async Task<ActionResult<List<AirportMaster>>> GetAllAirports()
    {
        _logger.LogInformation("Fetching all airports");
        return Ok(await _airportService.GetAllAirportsAsync());
    }

    [HttpGet("airports/{stateId}")]
    public async Task<ActionResult<List<AirportMaster>>> GetAirportsByStateId(int stateId)
    {
        _logger.LogInformation("Fetching airports for StateId: {StateId}", stateId);
        return Ok(await _airportService.GetAirportsByStateIdAsync(stateId));
    }
}
