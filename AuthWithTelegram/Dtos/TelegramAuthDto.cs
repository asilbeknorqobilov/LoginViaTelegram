using Microsoft.AspNetCore.Mvc;

namespace AuthWithTelegram.Dtos;

public class TelegramAuthDto
{
    [FromQuery(Name = "id")]
    public long Id { get; set; }

    [FromQuery(Name = "first_name")]
    public string FirstName { get; set; } = null!;

    [FromQuery(Name = "last_name")]
    public string? LastName { get; set; }

    [FromQuery(Name = "username")]
    public string? Username { get; set; }

    [FromQuery(Name = "photo_url")]
    public string? PhotoUrl { get; set; }

    [FromQuery(Name = "auth_date")]
    public long AuthDate { get; set; }

    [FromQuery(Name = "hash")]
    public string Hash { get; set; } = null!;
}