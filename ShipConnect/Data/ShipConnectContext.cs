using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShipConnect.Models;
using ShipConnect.RepositoryContract;
using System.Linq.Expressions;

namespace ShipConnect.Data
{
    public class ShipConnectContext : IdentityDbContext<ApplicationUser>
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

            // Apply soft delete filter to all entities that implement IBaseModel
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (typeof(IBaseModel).IsAssignableFrom(clrType))
                {
                    var parameter = Expression.Parameter(clrType, "e");
                    var property = Expression.Property(parameter, nameof(IBaseModel.IsDeleted));
                    var condition = Expression.Equal(property, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(clrType).HasQueryFilter(lambda);
                }
            }

            // Composite Key: Prevent duplicate rating
            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.UserId, r.RatedUserId, r.ShipmentId })
                .IsUnique();

            // One-to-one relationship: ApplicationUser <--> StartUp
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.StartupProfile)
                .WithOne(s => s.User)
                .HasForeignKey<StartUp>(s => s.UserId)
                .HasPrincipalKey<ApplicationUser>(u => u.Id);

            // One-to-one relationship: ApplicationUser <--> ShippingCompany
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.ShippingCompanyProfile)
                .WithOne(c => c.User)
                .HasForeignKey<ShippingCompany>(c => c.UserId)
                .HasPrincipalKey<ApplicationUser>(u => u.Id);

            // Prevent cascading delete in Rating
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

            // Optional: Prevent cascading delete between BankAccount and both StartUp & ShippingCompany
            modelBuilder.Entity<BankAccount>()
                .HasOne(b => b.ShippingCompany)
                .WithMany(c => c.BankAccounts)
                .HasForeignKey(b => b.ShippingCompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BankAccount>()
                .HasOne(b => b.StartUp)
                .WithMany(s => s.BankAccount)
                .HasForeignKey(b => b.StartUpId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IBaseModel &&
                            (e.State == EntityState.Added ||
                             e.State == EntityState.Modified ||
                             e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var entity = (IBaseModel)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Deleted)
                {
                    // Soft delete
                    entry.State = EntityState.Modified;
                    entity.IsDeleted = true;
                    entity.DeletedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
