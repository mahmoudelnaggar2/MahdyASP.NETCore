using Microsoft.EntityFrameworkCore;

namespace MahdyASP.NETCore.Data;

public class ApplicationDBContext(DbContextOptions options) : 
    DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<UserPermission>().ToTable("UserPermissions")
            .HasKey(x => new { x.UserId, x.PermissionId });
    }
}
