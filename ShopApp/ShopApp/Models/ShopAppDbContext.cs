using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShopApp.Models;

public partial class ShopAppDbContext : DbContext
{
    public ShopAppDbContext()
    {
    }

    public ShopAppDbContext(DbContextOptions<ShopAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<DanhGiaSanPham> DanhGiaSanPhams { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LichSuThemKhach> LichSuThemKhaches { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhienDangNhap> PhienDangNhaps { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<VwChiTietDonHangDayDu> VwChiTietDonHangDayDus { get; set; }

    public virtual DbSet<VwDonHangDayDu> VwDonHangDayDus { get; set; }

    public virtual DbSet<VwSanPhamConHang> VwSanPhamConHangs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=ShopApp;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChiTietD__3214EC07907536D0");

            entity.ToTable("ChiTietDonHang", tb => tb.HasTrigger("trg_CapNhatTongTien"));

            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.DonHang).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.DonHangId)
                .HasConstraintName("FK_ChiTiet_DonHang");

            entity.HasOne(d => d.SanPham).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTiet_SanPham");
        });

        modelBuilder.Entity<DanhGiaSanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DanhGiaS__3214EC0715D22732");

            entity.ToTable("DanhGiaSanPham");

            entity.Property(e => e.NgayDanhGia)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NoiDung).HasMaxLength(500);

            entity.HasOne(d => d.KhachHang).WithMany(p => p.DanhGiaSanPhams)
                .HasForeignKey(d => d.KhachHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DanhGia_KhachHang");

            entity.HasOne(d => d.SanPham).WithMany(p => p.DanhGiaSanPhams)
                .HasForeignKey(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_DanhGia_SanPham");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DonHang__3214EC079C37BFB3");

            entity.ToTable("DonHang");

            entity.HasIndex(e => e.MaDon, "UQ__DonHang__3D89F56910B3E656").IsUnique();

            entity.Property(e => e.MaDon)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("cho_xac_nhan");

            entity.HasOne(d => d.KhachHang).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.KhachHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonHang_KhachHang");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KhachHan__3214EC07495BBA63");

            entity.ToTable("KhachHang", tb => tb.HasTrigger("trg_LogThemKhach"));

            entity.HasIndex(e => e.Email, "UQ__KhachHan__A9D10534A86384A7").IsUnique();

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgayDangKy)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SDT");
        });

        modelBuilder.Entity<LichSuThemKhach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LichSuTh__3214EC07CEFD4586");

            entity.ToTable("LichSuThemKhach");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.ThoiGianThem)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhanVien__3214EC0710F85A17");

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.Email, "UQ__NhanVien__A9D10534C30E04B9").IsUnique();

            entity.Property(e => e.ChucVu).HasMaxLength(50);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
        });

        modelBuilder.Entity<PhienDangNhap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhienDan__3214EC077175D625");

            entity.ToTable("PhienDangNhap");

            entity.Property(e => e.DiaChiIp)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("DiaChiIP");
            entity.Property(e => e.ThoiGianDangNhap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.KhachHang).WithMany(p => p.PhienDangNhaps)
                .HasForeignKey(d => d.KhachHangId)
                .HasConstraintName("FK_Phien_KhachHang");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SanPham__3214EC0708FEFF56");

            entity.ToTable("SanPham", tb => tb.HasTrigger("trg_NganTonKhoAm"));

            entity.Property(e => e.DanhMuc).HasMaxLength(100);
            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Ten).HasMaxLength(200);
        });

        modelBuilder.Entity<VwChiTietDonHangDayDu>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ChiTietDonHangDayDu");

            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaDon)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenSanPham).HasMaxLength(200);
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(29, 2)");
        });

        modelBuilder.Entity<VwDonHangDayDu>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DonHangDayDu");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MaDon)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NgayDat).HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwSanPhamConHang>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_SanPhamConHang");

            entity.Property(e => e.DanhMuc).HasMaxLength(100);
            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Ten).HasMaxLength(200);
        });
        modelBuilder.HasSequence("sq_MaDon").StartsAt(100L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
