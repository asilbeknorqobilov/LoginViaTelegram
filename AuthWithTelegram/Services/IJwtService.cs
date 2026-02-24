using AuthWithTelegram.Models;

namespace AuthWithTelegram.Services;

public interface IJwtService
{
    string Generate(User user);
}