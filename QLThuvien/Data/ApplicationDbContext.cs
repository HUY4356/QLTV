using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Models;

namespace QLThuvien.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //public DbSet<QLThuvien.Models.User> User { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Role> DomainRoles { get; set; } = default!;
        // DbSet
        public DbSet<The> The { get; set; } = default!;
        public DbSet<HanMuc> HanMuc { get; set; } = default!;
        public object DatPhongs { get; internal set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // MAPPING cho bảng Role (domain)
            builder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Role__3213E83F8B0E788F");
                entity.ToTable("Role");
                entity.HasIndex(e => e.Name, "UQ__Role__72E12F1B162A7D2C").IsUnique();
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name");
            });

            // MAPPING cho bảng User (domain)
            builder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__User__3213E83F72144F6C");
                entity.ToTable("User");
                entity.HasIndex(e => e.Email, "UQ__User__AB6E6164424CEEE9").IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Fullname).HasMaxLength(100).HasColumnName("fullname");
                entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("email");
                entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("phone");
                entity.Property(e => e.Password).HasMaxLength(255).HasColumnName("password");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__role_id__49C3F6B7");
            });
        }

    }
}
