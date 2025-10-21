using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLThuvien.Models;

public partial class ThuVienDbContext : DbContext
{
    public ThuVienDbContext()
    {
    }

    public ThuVienDbContext(DbContextOptions<ThuVienDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BanTheSach> BanTheSaches { get; set; }

    public virtual DbSet<BaoCaoVongQuaySach> BaoCaoVongQuaySaches { get; set; }

    public virtual DbSet<BoSungTaiLieu> BoSungTaiLieus { get; set; }

    public virtual DbSet<DanhMucSach> DanhMucSaches { get; set; }

    public virtual DbSet<DatPhong> DatPhongs { get; set; }

    public virtual DbSet<DatTruocSach> DatTruocSaches { get; set; }

    public virtual DbSet<HanMuc> HanMucs { get; set; }

    public virtual DbSet<MuonTra> MuonTras { get; set; }

    public virtual DbSet<NhomSach> NhomSaches { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ThamSoHeThong> ThamSoHeThongs { get; set; }

    public virtual DbSet<The> Thes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ViPhamUser> ViPhamUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseSqlServer("Server=HUY;Database=ThuVienDB;Trusted_Connection=True;trustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BanTheSach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BanTheSa__3213E83F3B3E4872");

            entity.ToTable("BanTheSach");

            entity.HasIndex(e => e.MaVach, "UQ__BanTheSa__8BBF4A1C11D752B6").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DanhMucSachId).HasColumnName("DanhMucSach_id");
            entity.Property(e => e.MaVach).HasMaxLength(50);
            entity.Property(e => e.TinhTrang).HasMaxLength(20);

            entity.HasOne(d => d.DanhMucSach).WithMany(p => p.BanTheSaches)
                .HasForeignKey(d => d.DanhMucSachId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BanTheSac__DanhM__4222D4EF");
        });

        modelBuilder.Entity<BaoCaoVongQuaySach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BaoCaoVo__3213E83FD454339F");

            entity.ToTable("BaoCaoVongQuaySach");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DanhMucSachId).HasColumnName("DanhMucSach_id");
            entity.Property(e => e.ThoiGianBaoCao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.DanhMucSach).WithMany(p => p.BaoCaoVongQuaySaches)
                .HasForeignKey(d => d.DanhMucSachId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BaoCaoVon__DanhM__693CA210");
        });

        modelBuilder.Entity<BoSungTaiLieu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BoSungTa__3213E83FB2FE54A4");

            entity.ToTable("BoSungTaiLieu");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CapNhat).HasMaxLength(100);
            entity.Property(e => e.DanhMucSachId).HasColumnName("DanhMucSach_id");
            entity.Property(e => e.DeXuat).HasMaxLength(255);
            entity.Property(e => e.NoiDung).HasMaxLength(255);
            entity.Property(e => e.XuLy).HasMaxLength(100);

            entity.HasOne(d => d.DanhMucSach).WithMany(p => p.BoSungTaiLieus)
                .HasForeignKey(d => d.DanhMucSachId)
                .HasConstraintName("FK__BoSungTai__DanhM__6477ECF3");
        });

        modelBuilder.Entity<DanhMucSach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DanhMucS__3213E83FD6019CF9");

            entity.ToTable("DanhMucSach");

            entity.HasIndex(e => e.TacGia, "IX_Sach_TacGia");

            entity.HasIndex(e => e.TenSach, "IX_Sach_TenSach");

            entity.HasIndex(e => e.MaSach, "UQ__DanhMucS__B235742C011C7345").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DonGia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaSach).HasMaxLength(20);
            entity.Property(e => e.NhomSachId).HasColumnName("NhomSach_id");
            entity.Property(e => e.Slton).HasColumnName("SLTon");
            entity.Property(e => e.TacGia).HasMaxLength(255);
            entity.Property(e => e.TenSach).HasMaxLength(255);
            entity.Property(e => e.ViTriKe).HasMaxLength(50);

            entity.HasOne(d => d.NhomSach).WithMany(p => p.DanhMucSaches)
                .HasForeignKey(d => d.NhomSachId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhMucSa__NhomS__3D5E1FD2");
        });

        modelBuilder.Entity<DatPhong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DatPhong__3213E83FAAEB3603");

            entity.ToTable("DatPhong");

            entity.HasIndex(e => new { e.PhongId, e.GioBatDau, e.GioKetThuc }, "IX_DatPhong_Phong");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GioBatDau).HasColumnType("datetime");
            entity.Property(e => e.GioKetThuc).HasColumnType("datetime");
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhongId).HasColumnName("Phong_id");
            entity.Property(e => e.TrangThai).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Phong).WithMany(p => p.DatPhongs)
                .HasForeignKey(d => d.PhongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DatPhong__Phong___60A75C0F");

            entity.HasOne(d => d.User).WithMany(p => p.DatPhongs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DatPhong__user_i__619B8048");
        });

        modelBuilder.Entity<DatTruocSach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DatTruoc__3213E83F5D2CF91A");

            entity.ToTable("DatTruocSach");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DanhMucSachId).HasColumnName("DanhMucSach_id");
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.DanhMucSach).WithMany(p => p.DatTruocSaches)
                .HasForeignKey(d => d.DanhMucSachId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DatTruocS__DanhM__74AE54BC");

            entity.HasOne(d => d.User).WithMany(p => p.DatTruocSaches)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DatTruocS__user___75A278F5");
        });

        modelBuilder.Entity<HanMuc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HanMuc__3213E83F8306B867");

            entity.ToTable("HanMuc");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaxFines).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.HanMucs)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HanMuc__role_id__6FE99F9F");
        });

        modelBuilder.Entity<MuonTra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MuonTra__3213E83F0D752CBC");

            entity.ToTable("MuonTra");

            entity.HasIndex(e => e.TheId, "IX_MuonTra_The_Open").HasFilter("([TrangThai]='open')");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BanTheSachId).HasColumnName("BanTheSach_id");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.NgayMuon)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayTra).HasColumnType("datetime");
            entity.Property(e => e.TheId).HasColumnName("the_id");
            entity.Property(e => e.TienPhat)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(20);

            entity.HasOne(d => d.BanTheSach).WithMany(p => p.MuonTras)
                .HasForeignKey(d => d.BanTheSachId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MuonTra__BanTheS__5812160E");

            entity.HasOne(d => d.The).WithMany(p => p.MuonTras)
                .HasForeignKey(d => d.TheId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MuonTra__the_id__571DF1D5");
        });

        modelBuilder.Entity<NhomSach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhomSach__3213E83F7EE02729");

            entity.ToTable("NhomSach");

            entity.HasIndex(e => e.MaNhom, "UQ__NhomSach__234F91CC048CCA06").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaNhom).HasMaxLength(6);
            entity.Property(e => e.TenNhom).HasMaxLength(50);
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Phong__3213E83F169ACA9C");

            entity.ToTable("Phong");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DienTich).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TenPhong)
                .HasMaxLength(50)
                .HasColumnName("TenPhong");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83F8B0E788F");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "UQ__Role__72E12F1B162A7D2C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ThamSoHeThong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ThamSoHe__3213E83F18B7E309");

            entity.ToTable("ThamSoHeThong");

            entity.HasIndex(e => e.Ten, "UQ__ThamSoHe__C451FA839DD50A7D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GiaTri).HasMaxLength(100);
            entity.Property(e => e.Ten).HasMaxLength(100);
        });

        modelBuilder.Entity<The>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__The__3213E83FA205BDF9");

            entity.ToTable("The");

            entity.HasIndex(e => e.TheCode, "UQ__The__F4DC115952C80D47").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HanThe).HasColumnType("datetime");
            entity.Property(e => e.TheCode)
                .HasMaxLength(20)
                .HasColumnName("The_code");
            entity.Property(e => e.TrangThai).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Thes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__The__user_id__4E88ABD4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F72144F6C");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E6164424CEEE9").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .HasColumnName("fullname");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__role_id__49C3F6B7");
        });

        modelBuilder.Entity<ViPhamUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ViPhamUs__3213E83FD1117D98");

            entity.ToTable("ViPhamUser");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CountNoShow).HasDefaultValue(0);
            entity.Property(e => e.CountTreHan).HasDefaultValue(0);
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ViPhamUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ViPhamUse__user___7B5B524B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
