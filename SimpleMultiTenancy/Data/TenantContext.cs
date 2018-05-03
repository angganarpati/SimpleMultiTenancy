using SimpleMultiTenancy.Data.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace SimpleMultiTenancy.Data
{
    public class TenantContext : DbContext
    {
        public TenantContext()
            : base("TenantConnection")
        {
            Database.SetInitializer<TenantContext>(new TenantDBInitializer()); // Create Database If Not Exists
        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<TenantProfile> TenantProfile { get; set; }

        public DbSet<DBTenantConnectionString> DBTenantConnectionStrings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Tenant model builder

            modelBuilder.Entity<Tenant>()
                .HasKey(c => c.TenantID);

            modelBuilder.Entity<Tenant>()
                .Property(c => c.TenantName)
                .IsRequired()
                .HasMaxLength(128);

            modelBuilder.Entity<Tenant>()
                .Property(c => c.TenantCode)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute { IsUnique = true }));

            modelBuilder.Entity<Tenant>()
                .Property(c => c.ApplicationTitle)
                .IsRequired()
                .HasMaxLength(128);

            modelBuilder.Entity<Tenant>()
                .Property(c => c.CreatedBy).IsRequired().HasMaxLength(128);

            modelBuilder.Entity<Tenant>()
                .Property(c => c.UpdatedBy).HasMaxLength(128);

            #endregion Tenant model builder

            #region Tenant profile model builder

            modelBuilder.Entity<TenantProfile>()
                .Property(c => c.TenantId)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute { IsUnique = true }));

            modelBuilder.Entity<TenantProfile>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<Tenant>()
                .Property(c => c.CreatedBy).IsRequired().HasMaxLength(128);

            modelBuilder.Entity<Tenant>()
                .Property(c => c.UpdatedBy).HasMaxLength(128);

            #endregion Tenant profile model builder

            #region Tenant DB Configuration
            modelBuilder.Entity<DBTenantConnectionString>()
                .HasKey(c => c.TenantID);

            modelBuilder.Entity<DBTenantConnectionString>()
                .Property(c => c.DataSource)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<DBTenantConnectionString>()
                .Property(c => c.InitialCatalog)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<DBTenantConnectionString>()
                .Property(c => c.UserID)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<DBTenantConnectionString>()
                .Property(c => c.Password)
                .IsRequired()
                .HasMaxLength(256);
            #endregion
        }
    }
}