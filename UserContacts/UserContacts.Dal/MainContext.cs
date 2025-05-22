using Microsoft.EntityFrameworkCore;
using UserContacts.Dal.Entities;

namespace UserContacts.Dal;

public class MainContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public MainContext(DbContextOptions<MainContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainContext).Assembly);
    }
}
