using DotaProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DotaProject.Data.Repositories.DbContexts;

public class PlayerDbContext(DbContextOptions<PlayerDbContext> options) : DbContext(options)
{
    public DbSet<Player> Players => Set<Player>();
} 