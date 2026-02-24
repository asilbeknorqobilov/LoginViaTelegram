using AuthWithTelegram.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthWithTelegram.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
}