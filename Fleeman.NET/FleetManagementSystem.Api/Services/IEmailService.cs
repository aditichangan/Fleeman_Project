using System.Threading.Tasks;

namespace FleetManagementSystem.Api.Services;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string to, string token);
    Task SendInvoiceEmailWithAttachmentAsync(string to, byte[] pdfBytes, string filename, long bookingId);
}
