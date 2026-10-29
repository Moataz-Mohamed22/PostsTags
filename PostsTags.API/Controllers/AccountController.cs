using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PostsTags.Core.DTOs.auth;
using PostsTags.Core.Entities;
using PostsTags.Core.Interfaces;

namespace PostsTags.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> userManager;
        public AccountController(ITokenService tokenService, UserManager<AppUser> userManager)
        {
            _tokenService = tokenService;
            this.userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "User with this email already exists." });
            }
            var user = new AppUser
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim(),
                UserName = request.Email.Trim()
            };
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => e.Description)
                });
            }
            var (token, expiresAtUtc) = await _tokenService.CreateTokenAsync(user);
            return Ok(new AuthResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email!,
                Token = token,
                ExpiresAtUtc = expiresAtUtc
            });


        }

        [HttpPost("login")]
        public async Task<IActionResult>Login (LoginRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email.Trim());
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            var (token, expiresAtUtc) = await _tokenService.CreateTokenAsync(user);
            return Ok(new AuthResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email!,
                Token = token,
                ExpiresAtUtc = expiresAtUtc
            }); 
        }
    }
}
        

