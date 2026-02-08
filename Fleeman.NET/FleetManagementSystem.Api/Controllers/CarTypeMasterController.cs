using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FleetManagementSystem.Api.Models;
using FleetManagementSystem.Api.Services;
using Microsoft.Extensions.Logging;

namespace FleetManagementSystem.Api.Controllers;

[ApiController]
[Route("api/v1")]
public class CarTypeMasterController : ControllerBase
{
    private readonly ICarTypeMasterService _carTypeMasterService;
    private readonly ILogger<CarTypeMasterController> _logger;

    public CarTypeMasterController(ICarTypeMasterService carTypeMasterService, ILogger<CarTypeMasterController> logger)
    {
        _carTypeMasterService = carTypeMasterService;
        _logger = logger;
    }

    [HttpGet("car-types")]
    public async Task<ActionResult<List<CarTypeMaster>>> GetAllCarTypes()
    {
        _logger.LogInformation("Fetching all car types");
        return Ok(await _carTypeMasterService.GetAllCarTypesAsync());
    }
}
