using Microsoft.EntityFrameworkCore;
using TradeRisk.Domain.Entities;

namespace TradeRisk.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Trade> Trades => Set<Trade>();
}