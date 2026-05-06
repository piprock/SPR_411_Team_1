using Microsoft.EntityFrameworkCore;

namespace SPR_411_Team_1.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
