namespace DotaProject.Data;

using Microsoft.EntityFrameworkCore;
using DotaProject.Models;



public class UserDbContext(DbContextOptions<PlayerDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
} 