using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=localhost;Database=scooterdb;User Id=root;Password=root;Port=5432");
            }
        }

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
                    //.IsRequired()
                    .HasColumnType("json")
                    .HasColumnName("location");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.HasOne(d => d.IdZoneNavigation)
                    .WithMany(p => p.Scooters)
                    .HasForeignKey(d => d.IdZone)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("scooter-zone_fk");
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
