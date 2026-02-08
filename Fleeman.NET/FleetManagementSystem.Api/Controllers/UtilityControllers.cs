using Microsoft.AspNetCore.Mvc;
using FleetManagementSystem.Api.Models;
using FleetManagementSystem.Api.Services;
using FleetManagementSystem.Api.DTOs;
using System.Collections.Generic;

namespace FleetManagementSystem.Api.Controllers;

[ApiController]
[Route("api/v1")]
public class SupportController : ControllerBase
{
    private readonly ISupportService _supportService;
    public SupportController(ISupportService supportService) => _supportService = supportService;

    [HttpPost("support-tickets")]
    public async Task<ActionResult<SupportTicket>> CreateTicket([FromBody] SupportTicket ticket) => Ok(await _supportService.CreateTicketAsync(ticket));
    
    [HttpGet("support-tickets")]
    public async Task<ActionResult<List<SupportTicket>>> GetAllTickets() => Ok(await _supportService.GetAllTicketsAsync());
}

[ApiController]
[Route("api/v1")]
public class CustomerExistsController : ControllerBase
{
    private readonly ICheckCustomerExistsService _service;
    public CustomerExistsController(ICheckCustomerExistsService service) => _service = service;

    [HttpGet("customers/exists/{email}")]
    public async Task<ActionResult<bool>> CheckExists(string email)
    {
        return Ok(await _service.CustomerExistsAsync(email));
    }
}

[ApiController]
[Route("api/v1")]
public class GetCarDetailsFromBookingController : ControllerBase
{
    private readonly IGetCarDetailsFromBookingService _service;
    public GetCarDetailsFromBookingController(IGetCarDetailsFromBookingService service) => _service = service;

    [HttpGet("booking/{bookingId}/car")]
    public async Task<ActionResult<CarMaster>> GetCar(long bookingId)
    {
        var car = await _service.GetCarDetailsAsync(bookingId);
        if (car != null) return Ok(car);
        return NotFound();
    }
}

[ApiController]
[Route("api/v1")]
public class LocaleController : ControllerBase
{
    private readonly ILocaleService _service;
    public LocaleController(ILocaleService service) => _service = service;
    
    [HttpGet("locale/{lang}")]
    public ActionResult<string> GetLocale(string lang) => Ok(_service.GetLocaleData(lang));
}
