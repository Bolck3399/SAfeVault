using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<VaultItem> VaultItems => Set<VaultItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}