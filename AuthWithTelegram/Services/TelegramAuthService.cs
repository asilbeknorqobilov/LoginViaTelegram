using System.Security.Cryptography;
using System.Text;
using AuthWithTelegram.Dtos;

namespace AuthWithTelegram.Services;

public class TelegramAuthService
{
    private readonly string _botToken;

    public TelegramAuthService(IConfiguration configuration)
    {
        _botToken = configuration["Telegram:BotToken"]!;
    }

    public bool Validate(TelegramAuthDto dto)
    {
        var data = new SortedDictionary<string, string>
        {
            { "auth_date", dto.AuthDate.ToString() },
            { "first_name", dto.FirstName },
            { "id", dto.Id.ToString() }
        };

        if (!string.IsNullOrEmpty(dto.LastName))
            data.Add("last_name", dto.LastName);

        if (!string.IsNullOrEmpty(dto.Username))
            data.Add("username", dto.Username);

        if (!string.IsNullOrEmpty(dto.PhotoUrl))
            data.Add("photo_url", dto.PhotoUrl);

        var dataCheckString = string.Join("\n", data.Select(x => $"{x.Key}={x.Value}"));

        using var sha256 = SHA256.Create();
        var secretKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(_botToken));

        using var hmac = new HMACSHA256(secretKey);
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));
        var calculatedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        return calculatedHash == dto.Hash;
    }
}