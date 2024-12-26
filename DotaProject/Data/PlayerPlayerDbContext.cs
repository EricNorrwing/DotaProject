using Microsoft.EntityFrameworkCore;
using DotaProject.Models;

namespace DotaProject.Data;

public class PlayerDbContext(DbContextOptions<PlayerDbContext> options) : DbContext(options)
{
    public DbSet<Player> Players => Set<Player>();
} 