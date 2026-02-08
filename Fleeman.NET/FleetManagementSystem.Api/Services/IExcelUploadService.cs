using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FleetManagementSystem.Api.Services;

public interface IExcelUploadService
{
    Task SaveAsync(IFormFile file); 
    Task SaveRatesAsync(IFormFile file);
    Task SaveCarsAsync(IFormFile file);
}
