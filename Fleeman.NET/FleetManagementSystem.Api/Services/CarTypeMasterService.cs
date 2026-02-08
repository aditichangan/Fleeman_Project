using Microsoft.EntityFrameworkCore;
using FleetManagementSystem.Api.Data;
using FleetManagementSystem.Api.Models;

namespace FleetManagementSystem.Api.Services;

public class CarTypeMasterService : ICarTypeMasterService
{
    private readonly ApplicationDbContext _context;

    public CarTypeMasterService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CarTypeMaster>> GetAllCarTypesAsync()
    {
        return await _context.CarTypes.ToListAsync();
    }
}
