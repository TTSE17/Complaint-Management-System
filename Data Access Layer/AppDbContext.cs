using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Data_Access_Layer;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public new DbSet<User> Users { get; set; }
    public DbSet<Citizen> Citizens { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Complaint> Complaints { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}