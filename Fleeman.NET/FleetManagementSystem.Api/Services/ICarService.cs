using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FleetManagementSystem.Api.Models;

namespace FleetManagementSystem.Api.Services;

public interface ICarService
{
    Task<List<object[]>> GetCarsByHubAddressAsync(string hubAddress);
    Task<List<CarMaster>> GetAvailableCarsAsync(int hubId, DateTime startDate, DateTime endDate, long? carTypeId);
}
