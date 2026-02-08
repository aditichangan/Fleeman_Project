using Microsoft.AspNetCore.Mvc;
using FleetManagementSystem.Api.Models;
using FleetManagementSystem.Api.Services;
using FleetManagementSystem.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FleetManagementSystem.Api.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;
    private readonly ApplicationDbContext _context; // Replacing CustomerRepository
    private readonly IGoogleAuthService _googleAuthService;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, IJwtService jwtService, ApplicationDbContext context, IGoogleAuthService googleAuthService, IEmailService emailService, ILogger<UserController> logger)
    {
        _userService = userService;
        _jwtService = jwtService;
        _context = context;
        _googleAuthService = googleAuthService;
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new User
        {
            Username = request.Username,
            Password = request.Password,
            Email = request.Email,
            Role = Role.CUSTOMER.ToString()
        };

        var createdUser = await _userService.AddUserAsync(user);
        if (createdUser == null)
        {
            return BadRequest("User creation failed. Username or Email might already exist.");
        }
        return Created("", createdUser); 
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        _logger.LogInformation("Login attempt for user: {Username}", loginRequest.Username);
        try
        {
            if (await _userService.ValidateCredentialsAsync(loginRequest.Username, loginRequest.Password))
            {
                // Authenticated
                 var jwt = _jwtService.GenerateToken(loginRequest.Username);
                 var fullUser = await _userService.GetUserByUsernameAsync(loginRequest.Username);

                 var response = new Dictionary<string, object>
                 {
                     { "token", jwt },
                     { "role", fullUser.Role },
                     { "userId", fullUser.Id },
                     { "email", fullUser.Email },
                     { "username", fullUser.Username }
                 };

                 if (fullUser.HubId.HasValue)
                 {
                     response["hubId"] = fullUser.HubId;
                 }

                 var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == fullUser.Email);
                 if (customer != null)
                 {
                     response["customerId"] = customer.CustId;
                 }
                 else
                 {
                     response["customerId"] = fullUser.Id;
                 }

                 return Ok(response);
            }
            else
            {
                _logger.LogWarning("Invalid login attempt for user: {Username}", loginRequest.Username);
                return Unauthorized("Authentication failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Username}", loginRequest.Username);
            return Unauthorized("Invalid Creds");
        }
    }

    [HttpPost("api/v1/auth/google")]
    public async Task<IActionResult> GoogleLogin([FromBody] Dictionary<string, string> request)
    {
        try
        {
            if (request.TryGetValue("token", out var token))
            {
                var authResponse = await _googleAuthService.VerifyGoogleTokenAndGetJwtAsync(token);
                return Ok(authResponse);
            }
             return Unauthorized("Invalid Google Token");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error: " + ex.Message + " | " + ex.InnerException?.Message);
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] Dictionary<string, string> request)
    {
        if (request.TryGetValue("email", out var email))
        {
            var token = await _userService.GenerateResetTokenAsync(email);
            if (token != null)
            {
                await _emailService.SendPasswordResetEmailAsync(email, token);
            }
        }
        return Ok("If an account exists with this email, you will receive a reset link shortly.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] Dictionary<string, string> request)
    {
        var token = request.TryGetValue("token", out var t) ? t : null;
        var password = request.TryGetValue("password", out var p) ? p : null;

        if (token != null && password != null && await _userService.ResetPasswordAsync(token, password))
        {
            return Ok("Password has been reset successfully.");
        }
        return BadRequest("Invalid or expired reset token.");
    }
}
