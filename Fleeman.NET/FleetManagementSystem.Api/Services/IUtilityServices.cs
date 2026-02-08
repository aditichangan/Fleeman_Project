using FleetManagementSystem.Api.Models;
using System.Collections.Generic;

namespace FleetManagementSystem.Api.Services;

using System.Threading.Tasks;

public interface ISupportService
{
    Task<SupportTicket> CreateTicketAsync(SupportTicket ticket);
    Task<List<SupportTicket>> GetAllTicketsAsync();
    Task<SupportTicket> UpdateTicketAsync(int id, SupportTicket ticket);
}

public interface ICheckCustomerExistsService
{
    Task<bool> CustomerExistsAsync(string email);
}

public interface IGetCarDetailsFromBookingService
{
    Task<CarMaster> GetCarDetailsAsync(long bookingId);
}

public interface ILocaleService
{
    // Stub for locale logic
    string GetLocaleData(string lang);
}
