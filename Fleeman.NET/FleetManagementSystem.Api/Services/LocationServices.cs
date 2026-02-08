using System.Collections.Generic;
using System.Linq;
using FleetManagementSystem.Api.Data;
using FleetManagementSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManagementSystem.Api.Services;

public class StateService : IStateService
{
    private readonly ApplicationDbContext _context;
    public StateService(ApplicationDbContext context) => _context = context;
    public async Task<List<StateMaster>> GetAllStatesAsync() => await _context.States.ToListAsync();
}

public class CityService : ICityService
{
    private readonly ApplicationDbContext _context;
    public CityService(ApplicationDbContext context) => _context = context;
    public async Task<List<CityMaster>> GetAllCitiesAsync() => await _context.Cities.Include(c => c.State).ToListAsync();
    public async Task<List<CityMaster>> GetCitiesByStateIdAsync(int stateId) => await _context.Cities.Where(c => c.StateId == stateId).Include(c => c.State).ToListAsync();
}

public class HubService : IHubService
{
    private readonly ApplicationDbContext _context;
    public HubService(ApplicationDbContext context) => _context = context;
    public async Task<List<HubMaster>> GetAllHubsAsync() => await _context.Hubs.Include(h => h.City).Include(h => h.City.State).ToListAsync();
    public async Task<List<HubMaster>> GetHubsByCityIdAsync(int cityId) => await _context.Hubs.Where(h => h.CityId == cityId).Include(h => h.City).Include(h => h.City.State).ToListAsync();
    public async Task<HubMaster> GetHubByIdAsync(int hubId) => await _context.Hubs.Include(h => h.City).Include(h => h.City.State).FirstOrDefaultAsync(h => h.HubId == hubId);
    
    public async Task<List<HubMaster>> SearchHubsByAirportCodeAsync(string airportCode)
    {
        var hubIds = await _context.Airports
            .Where(a => a.AirportCode == airportCode && a.HubId != null)
            .Select(a => a.HubId)
            .ToListAsync();

        return await _context.Hubs
            .Where(h => hubIds.Contains(h.HubId))
            .Include(h => h.City)
            .Include(h => h.City.State)
            .ToListAsync();
    }
}

public class AirportService : IAirportService
{
    private readonly ApplicationDbContext _context;
    public AirportService(ApplicationDbContext context) => _context = context;
    public async Task<List<AirportMaster>> GetAllAirportsAsync() => await _context.Airports.Include(a => a.City).Include(a => a.State).Include(a => a.Hub).ToListAsync();
    public async Task<List<AirportMaster>> GetAirportsByStateIdAsync(int stateId) => await _context.Airports.Where(a => a.StateId == stateId).Include(a => a.City).Include(a => a.State).Include(a => a.Hub).ToListAsync();
}
