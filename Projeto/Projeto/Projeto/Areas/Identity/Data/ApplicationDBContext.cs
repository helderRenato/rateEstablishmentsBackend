using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projeto.Models;
namespace Projeto.Areas.Identity.Data;

public class ApplicationDBContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    // define table on the database
    public DbSet<Establishment> Establishment { get; set; }
    public DbSet<Rating> Rating { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<Photo> Photo { get; set; }
    public DbSet<User> User { get; set; }

}
