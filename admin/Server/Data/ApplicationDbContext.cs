using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using Application.Shared.Models;
using Application.Shared.Models.Org;
using Application.Shared.Models.Outbound;
using Microsoft.AspNetCore.Identity;

namespace Application.Server.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("ApplicationUser");
            });
            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaim");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogin");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("Token");
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Role");
            });

            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaim");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRole");
            });
    }

    public DbSet<Application.Shared.Models.ApplicationUser> ApplicationUser { get; set; }
    public DbSet<Application.Shared.Models.Org.Entity> Entity { get; set; }

    public DbSet<Application.Shared.Models.Org.Store> Store { get; set; }

    public DbSet<Application.Shared.Models.Outbound.RouteHeader> RouteHeader { get; set; }
    public DbSet<Application.Shared.Models.Outbound.RouteLine> RouteLine { get; set; }
    public DbSet<Application.Shared.Models.Org.Driver> Driver { get; set; }
    public DbSet<Application.Shared.Models.Org.Truck> Truck { get; set; }
    public DbSet<Application.Shared.Models.Outbound.Consolidation> Consolidation { get; set; }
    public DbSet<Application.Shared.Models.Outbound.LoadHeader> LoadHeader { get; set; }
    public DbSet<Application.Shared.Models.Outbound.LoadLine> LoadLine { get; set; }
    public DbSet<Application.Shared.Models.Outbound.LicensePlate> LicensePlate { get; set; }
    public DbSet<Application.Shared.Models.Warehouse.WorkDetail> WorkDetail { get; set; }
    public DbSet<Application.Shared.Models.Org.UserStore> UserStore { get; set; }
}
