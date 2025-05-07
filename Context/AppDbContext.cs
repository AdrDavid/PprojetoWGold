

using ApiWgold.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiWgold.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Game>? Game { get; set; }
    public DbSet<GoldListing>? GoldListing { get; set; }
    public DbSet<Order>? Order { get; set; }
    public DbSet<Server>? Server { get; set; }
    public DbSet<User>? User { get; set; }
    //e faz as outras
}
