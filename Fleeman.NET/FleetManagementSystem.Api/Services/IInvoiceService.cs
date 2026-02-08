using System.Threading.Tasks;

namespace FleetManagementSystem.Api.Services;

public interface IInvoiceService
{
    Task SendInvoiceEmailAsync(long bookingId, string email);
}
