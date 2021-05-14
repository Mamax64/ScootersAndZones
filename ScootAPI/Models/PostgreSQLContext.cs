using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScootAPI.Models
{
    public partial class PostgreSQLContext : DbContext
    {
        public PostgreSQLContext()
        {
        }

        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Scooter> Scooters { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Scooter>(entity =>
            {
                entity.HasKey(e => e.IdScooter)
                    .HasName("scooter_pkey");

                entity.ToTable("scooter");

                entity.Property(e => e.IdScooter)
                    .HasMaxLength(50)
                    .HasColumnName("idScooter");

                entity.Property(e => e.IdZone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("idZone");

                entity.Property(e => e.IsDisabled).HasColumnName("isDisabled");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnType("jsonb")
                    .HasColumnName("location");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Zone>(entity =>
            {
                entity.HasKey(e => e.IdZone)
                    .HasName("zone_pkey");

                entity.ToTable("zone");

                entity.Property(e => e.IdZone)
                    .HasMaxLength(50)
                    .HasColumnName("idZone");

                entity.Property(e => e.IsDisabled).HasColumnName("isDisabled");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
