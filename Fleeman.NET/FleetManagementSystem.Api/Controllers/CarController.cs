using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using FleetManagementSystem.Api.Models;
using FleetManagementSystem.Api.Services;
using FleetManagementSystem.Api.DTOs;
using Microsoft.Extensions.Logging;

namespace FleetManagementSystem.Api.Controllers;

[ApiController]
[Route("api/v1")]
public class CarController : ControllerBase
{
    private readonly ICarService _carService;
    private readonly IExcelUploadService _excelUploadService;
    private readonly ILogger<CarController> _logger;

    public CarController(ICarService carService, IExcelUploadService excelUploadService, ILogger<CarController> logger)
    {
        _carService = carService;
        _excelUploadService = excelUploadService;
        _logger = logger;
    }

    [HttpPost("cars/upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        string message = "";
        try
        {
            await _excelUploadService.SaveCarsAsync(file);
            message = "Uploaded the file successfully: " + file.FileName;
            return Ok(new MessageResponse(message));
        }
        catch (Exception)
        {
            message = "Could not upload the file: " + file.FileName + "!";
            return StatusCode(StatusCodes.Status417ExpectationFailed, new MessageResponse(message));
        }
    }

    [HttpGet("cars")]
    public async Task<ActionResult<List<object[]>>> GetCarDetailsByHubAddress([FromQuery] string hubAddress)
    {
        var cars = await _carService.GetCarsByHubAddressAsync(hubAddress);
        return Ok(cars);
    }

    [HttpGet("cars/available")]
    public async Task<ActionResult<List<CarMaster>>> GetAvailableCars(
            [FromQuery] int hubId,
            [FromQuery] string startDate,
            [FromQuery] string endDate,
            [FromQuery] long? carTypeId = null)
    {
        _logger.LogInformation("Searching available cars for HubId: {HubId}, Dates: {Start} to {End}", hubId, startDate, endDate);
        // Parsing dates manually as in Java
        if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end))
        {
             var cars = await _carService.GetAvailableCarsAsync(hubId, start, end, carTypeId);
             return Ok(cars);
        }
        return BadRequest("Invalid date format");
    }
}
