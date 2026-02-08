using System.Threading.Tasks;
using FleetManagementSystem.Api.Models;

namespace FleetManagementSystem.Api.Services;

public interface IUserService
{
    Task<User> AddUserAsync(User user);
    Task<User> GetUserByUsernameAsync(string username);
    Task<string> GenerateResetTokenAsync(string email);
    Task<bool> ResetPasswordAsync(string token, string newPassword);
    Task<bool> ValidateCredentialsAsync(string username, string password);
}
