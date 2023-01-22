using Microsoft.EntityFrameworkCore;

public class RestSampleContext : DbContext
{
    public RestSampleContext(DbContextOptions<RestSampleContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    public DbSet<User> Users { get; set; } = null!;
}