using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;
using System.Linq.Expressions;

namespace ShipConnect.Data
{
    public class ShipConnectContext:IdentityDbContext<ApplicationUser>
    {
        public ShipConnectContext(DbContextOptions<ShipConnectContext> options) : base(options) { }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }  
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Receiver> Receivers { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShippingCompany> ShippingCompanies { get; set; }
        public DbSet<StartUp> StartUps { get; set; }
        public DbSet<Tracking> Trackings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (typeof(IBaseModel).IsAssignableFrom(clrType))
                {
                    var parameter = Expression.Parameter(clrType, "e");

                    var property = Expression.Property(parameter, "IsDeleted");

                    var condition = Expression.Equal(property, Expression.Constant(false));

                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(clrType).HasQueryFilter(lambda);
                }
            }

            #region Rating Composite Key
            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.UserId, r.RatedUserId, r.ShipmentId })
                .IsUnique();

            base.OnModelCreating(modelBuilder);

            #endregion
            
            #region One-to-one relationships
            modelBuilder.Entity<ApplicationUser>()
        .HasOne(u => u.StartupProfile)
        .WithOne(s => s.User)
        .HasForeignKey<StartUp>(s => s.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.ShippingCompanyProfile)
                .WithOne(c => c.User)
                .HasForeignKey<ShippingCompany>(c => c.UserId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.RatedUser)
                .WithMany(u => u.RatedUser)
                .HasForeignKey(r => r.RatedUserId)
                .OnDelete(DeleteBehavior.Restrict); 
            #endregion
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IBaseModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (IBaseModel)entry.Entity;
                if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Deleted)
                {
                    entity.DeletedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
