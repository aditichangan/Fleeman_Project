using System.Linq;
using System.Collections.Generic;
using FleetManagementSystem.Api.Data;
using FleetManagementSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManagementSystem.Api.Services;

public class SupportService : ISupportService
{
    private readonly ApplicationDbContext _context;
    public SupportService(ApplicationDbContext context) => _context = context;
    public async Task<SupportTicket> CreateTicketAsync(SupportTicket ticket)
    {
        _context.SupportTickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }
    public async Task<List<SupportTicket>> GetAllTicketsAsync() => await _context.SupportTickets.ToListAsync();
    public async Task<SupportTicket> UpdateTicketAsync(int id, SupportTicket ticket)
    {
         var existing = await _context.SupportTickets.FindAsync(id);
         if (existing != null)
         {
              // Update all fields or specific ones
              existing.Status = ticket.Status; // Example
              // ... map others
              await _context.SaveChangesAsync();
              return existing;
         }
         return null;
    }
}

public class CheckCustomerExistsService : ICheckCustomerExistsService
{
    private readonly ApplicationDbContext _context;
    public CheckCustomerExistsService(ApplicationDbContext context) => _context = context;

    public async Task<bool> CustomerExistsAsync(string email)
    {
        return await _context.Customers.AnyAsync(c => c.Email == email);
    }
}

public class GetCarDetailsFromBookingService : IGetCarDetailsFromBookingService
{
    private readonly ApplicationDbContext _context;
    public GetCarDetailsFromBookingService(ApplicationDbContext context) => _context = context;

    public async Task<CarMaster> GetCarDetailsAsync(long bookingId)
    {
        var booking = await _context.Bookings.Include(b => b.Car).FirstOrDefaultAsync(b => b.BookingId == bookingId);
        return booking?.Car;
    }
}

public class LocaleService : ILocaleService
{
    public string GetLocaleData(string lang)
    {
        // Return dummy json or similar, or just stub
        return "{}";
    }
}
