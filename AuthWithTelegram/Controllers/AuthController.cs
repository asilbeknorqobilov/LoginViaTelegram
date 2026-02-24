using AuthWithTelegram.Data;
using AuthWithTelegram.Dtos;
using AuthWithTelegram.Models;
using AuthWithTelegram.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthWithTelegram.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController:ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TelegramAuthService _telegramService;
    private readonly IJwtService _jwtService;

    public AuthController(
        AppDbContext context,
        TelegramAuthService telegramService,
        IJwtService jwtService)
    {
        _context = context;
        _telegramService = telegramService;
        _jwtService = jwtService;
    }

    [HttpGet("telegram")]
    public async Task<IActionResult> TelegramLogin([FromQuery] TelegramAuthDto dto)
    {
        if (!_telegramService.Validate(dto))
            return Unauthorized("Invalid Telegram login");

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.TelegramId == dto.Id);

        if (user == null)
        {
            user = new User
            {
                TelegramId = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                PhotoUrl = dto.PhotoUrl
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        var token = _jwtService.Generate(user);

        return Ok(new { token });
    }
}