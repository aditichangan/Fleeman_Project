using System.Collections.Generic;
using System.Threading.Tasks;
using FleetManagementSystem.Api.DTOs;

namespace FleetManagementSystem.Api.Services;

public interface IBookingService
{
    Task<BookingResponse> CreateBooking(BookingRequest request);
    Task<BookingResponse> ProcessHandover(HandoverRequest request);
    Task<BookingResponse> ReturnCar(ReturnRequest request);
    Task<BookingResponse> GetBooking(string bookingId);
    Task<List<BookingResponse>> GetBookingsByEmail(string email);
    Task<List<BookingResponse>> GetAllBookings();
    Task<List<BookingResponse>> GetBookingsByHub(int hubId);
    Task<BookingResponse> CancelBooking(long bookingId);
    Task<BookingResponse> ModifyBooking(long bookingId, BookingRequest request);
}
