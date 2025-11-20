using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Data_Access_Layer;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public new DbSet<User> Users { get; set; }
    public DbSet<Citizen> Clients { get; set; }
    
    public DbSet<Department> Departments { get; set; }
    
    public DbSet<Complaint> Complaints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}