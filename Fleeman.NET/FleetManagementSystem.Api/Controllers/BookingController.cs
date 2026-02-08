using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FleetManagementSystem.Api.DTOs;
using FleetManagementSystem.Api.Services;
using Microsoft.Extensions.Logging;

namespace FleetManagementSystem.Api.Controllers;

[ApiController]
[Route("booking")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingController> _logger;

    public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [HttpGet("heartbeat")]
    public IActionResult Heartbeat() => Ok("Booking Controller is Running - Version 3 (Get Fix)");

    [HttpGet("user/{email}")]
    public async Task<ActionResult<List<BookingResponse>>> GetBookingsByUser(string email)
    {
        return Ok(await _bookingService.GetBookingsByEmail(email));
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<BookingResponse>>> GetAllBookings()
    {
        return Ok(await _bookingService.GetAllBookings());
    }

    [HttpGet("hub/{hubId}")]
    public async Task<ActionResult<List<BookingResponse>>> GetBookingsByHub(int hubId)
    {
        return Ok(await _bookingService.GetBookingsByHub(hubId));
    }

    [HttpPost("create")]
    public async Task<ActionResult<BookingResponse>> CreateBooking([FromBody] BookingRequest request)
    {
        _logger.LogInformation("Creating booking for customer: {Email}, CarId: {CarId}", request.Email, request.CarId);
        try
        {
            return Ok(await _bookingService.CreateBooking(request));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for customer: {Email}", request.Email);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("handover/{id}")]
    public async Task<ActionResult<BookingResponse>> HandoverCar([FromRoute] long id)
    {
        var req = new HandoverRequest { BookingId = id };
        try
        {
            return Ok(await _bookingService.ProcessHandover(req));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error processing handover for booking ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("process-handover")]
    public async Task<ActionResult<BookingResponse>> ProcessHandover([FromBody] HandoverRequest request)
    {
        try
        {
            return Ok(await _bookingService.ProcessHandover(request));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing handover for booking ID: {Id}", request.BookingId);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("return")]
    public async Task<ActionResult<BookingResponse>> ReturnCar([FromBody] ReturnRequest request)
    {
        try
        {
            return Ok(await _bookingService.ReturnCar(request));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing return for booking ID: {Id}", request.BookingId);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("cancel/{id}")]
    public async Task<ActionResult<BookingResponse>> CancelBooking([FromRoute] long id)
    {
        try
        {
            return Ok(await _bookingService.CancelBooking(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("modify/{id}")]
    public async Task<ActionResult<BookingResponse>> ModifyBooking([FromRoute] long id, [FromBody] BookingRequest request)
    {
        _logger.LogInformation("Modifying booking ID: {Id}", id);
        try
        {
             return Ok(await _bookingService.ModifyBooking(id, request));
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Error modifying booking ID: {Id}", id);
             return BadRequest(ex.Message);
        }
    }

    [HttpPost("storeDates")]
    public ActionResult<string> StoreBookingDates([FromQuery] string start_date, [FromQuery] string end_date, [FromQuery] int cust_id)
    {
        return Ok("success");
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<BookingResponse>> GetBooking([FromRoute] string id)
    {
        try
        {
            var response = await _bookingService.GetBooking(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
