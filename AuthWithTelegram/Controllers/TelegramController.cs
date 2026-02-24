using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace AuthWithTelegram.Controllers;

public class TelegramUserDto
{
    public string Id { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string? LastName { get; set; }
    public string Username { get; set; } = "";
    public string AuthDate { get; set; } = "";
    public string Hash { get; set; } = "";
}

[ApiController]
[Route("auth")]
public class TelegramController:ControllerBase
{
    private readonly string botToken = "8315906612:AAH-ZxZOYUi5CaF_v_O2fljIhWNd10ODAPs";

    [HttpGet("telegram")]
    public IActionResult TelegramAuth([FromQuery] TelegramUserDto user)
    {
        if (!VerifyTelegramSignature(user))
            return BadRequest("Invalid signature");

        // Bu yerda foydalanuvchini bazaga saqlash yoki session yaratish mumkin
        return Ok($"Salom, {user.FirstName}! Siz tizimga kirdingiz.");
    }

    private bool VerifyTelegramSignature(TelegramUserDto user)
    {
        // Telegram hash tekshirish
        var dataCheckArr = new List<string>();
        if (!string.IsNullOrEmpty(user.Id)) dataCheckArr.Add($"id={user.Id}");
        if (!string.IsNullOrEmpty(user.FirstName)) dataCheckArr.Add($"first_name={user.FirstName}");
        if (!string.IsNullOrEmpty(user.LastName)) dataCheckArr.Add($"last_name={user.LastName}");
        if (!string.IsNullOrEmpty(user.Username)) dataCheckArr.Add($"username={user.Username}");
        if (!string.IsNullOrEmpty(user.AuthDate)) dataCheckArr.Add($"auth_date={user.AuthDate}");

        dataCheckArr.Sort();
        var dataCheckString = string.Join("\n", dataCheckArr);

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(botToken));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));
        var hashHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        return hashHex == user.Hash.ToLower();
    }
}
