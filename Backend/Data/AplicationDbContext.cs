using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<NotasInternas> NotasInternas { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.IsAdmin)
                .IsRequired()
                .HasDefaultValue(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(e => e.Rol)
                    .HasMaxLength(50);

                entity.HasIndex(e => e.Email).IsUnique();

            });
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasDefaultValue("Enviado");

                entity.Property(e => e.Location)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

            });
            modelBuilder.Entity<Service>(entity =>
           {
               entity.HasKey(e => e.Id);

               entity.Property(e => e.Type)
                   .IsRequired()
                   .HasMaxLength(50);

               entity.Property(e => e.Company)
                   .IsRequired()
                   .HasMaxLength(200);
           });
            modelBuilder.Entity<NotasInternas>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);
            });
        }
    }
}