using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DemoApp.Models;

namespace DemoApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<DemoApp.Models.User> User { get; set; } = default!;
        public DbSet<DemoApp.Models.KhoaHoc>? KhoaHoc { get; set; }
        public DbSet<DemoApp.Models.BaiHoc>? BaiHoc { get; set; }
        public DbSet<DemoApp.Models.DangKyKhoaHoc>? DangKyKhoaHoc { get; set; }
        public DbSet<DemoApp.Models.DanhMuc>? DanhMuc { get; set; }
        public DbSet<DemoApp.Models.Role>? Role { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.RoleId);
                entity.HasMany(e => e.KhoaHocDay)
                .WithOne(e => e.user)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);


                entity.HasMany(e => e.DangKyKhoaHocs)
                    .WithOne(e => e.user)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
            );

            modelBuilder.Entity<Role>(entity => 
            {
                entity.HasIndex(e => e.RoleName).IsUnique();
            });
            modelBuilder.Entity<DanhMuc>(entity =>
            {
                entity.Property(e => e.TrangThai)
                    .HasDefaultValue(true);
            });
            modelBuilder.Entity<KhoaHoc>(entity =>
            {
                entity.HasIndex(e => e.MaKhoaHoc).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.DanhMucId);
                entity.HasIndex(e => e.TrangThai);

                entity.Property(e => e.CapDo)
                    .HasDefaultValue("CoBan");

                entity.Property(e => e.TrangThai)
                    .HasDefaultValue("BanNhap");

                entity.Property(e => e.GiaTien)
                    .HasPrecision(18, 2)
                    .HasDefaultValue(0);


                entity.HasOne(e => e.DanhMuc)
                    .WithMany(e => e.KhoaHoc)
                    .HasForeignKey(e => e.DanhMucId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<BaiHoc>(entity =>
            {
                entity.HasIndex(e => e.KhoaHocId);

                entity.Property(e => e.LoaiNoiDung)
                    .HasDefaultValue("Video");


                entity.HasOne(e => e.KhoaHoc)
                    .WithMany(e => e.BaiHoc)
                    .HasForeignKey(e => e.KhoaHocId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<DangKyKhoaHoc>(entity =>
            {
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.KhoaHocId);
                entity.HasIndex(e => new { e.UserId, e.KhoaHocId }).IsUnique();

                entity.Property(e => e.TrangThai)
                    .HasDefaultValue("DangHoc");

                entity.HasOne(e => e.user)
                    .WithMany(e => e.DangKyKhoaHocs)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.KhoaHoc)
                    .WithMany(e => e.DangKyKhoaHoc)
                    .HasForeignKey(e => e.KhoaHocId)
                    .OnDelete(DeleteBehavior.Restrict);

               
            });
            SeedData(modelBuilder);
        }
            private void SeedData(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "User" }
            );

           
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    Email = "admin@khoahoc.vn",
                    Password = "admin123",
                    FullName = "Quản Trị Viên",
                    RoleId = 1,
                    Address = "Hà Nội",
                    NumberPhone = "0123456789"
                },
                new User
                {
                    UserId = 2,
                    Username = "user01",
                    Email = "user01@khoahoc.vn",
                    Password = "user123",
                    FullName = "Nguyễn Văn A",
                    RoleId = 2,
                    Address = "TP HCM",
                    NumberPhone = "0987654321"
                },
                new User
                {
                    UserId = 3,
                    Username = "user02",
                    Email = "user02@gmail.com",
                    Password = "user321",
                    FullName = "Lê Văn C",
                    RoleId = 2,
                    Address = "Đà Nẵng",
                    NumberPhone = "0912345678"
                }
            );

           
            modelBuilder.Entity<DanhMuc>().HasData(
                new DanhMuc
                {
                    Id = 1,
                    TenDanhMuc = "Lập Trình Web",
                    MoTa = "Học phát triển website",
                    ThuTuHienThi = 1,
                    TrangThai = true
                },
                new DanhMuc
                {
                    Id = 2,
                    TenDanhMuc = "Lập Trình Mobile",
                    MoTa = "Phát triển ứng dụng di động",
                    ThuTuHienThi = 2,
                    TrangThai = true
                },
                new DanhMuc
                {
                    Id = 3,
                    TenDanhMuc = "Cơ Sở Dữ Liệu",
                    MoTa = "SQL Server, MySQL",
                    ThuTuHienThi = 3,
                    TrangThai = true
                }
            );

           
            modelBuilder.Entity<KhoaHoc>().HasData(
                new KhoaHoc
                {
                    Id = 1,
                    MaKhoaHoc = "WEB001",
                    TenKhoaHoc = "HTML CSS JavaScript Cơ Bản",
                    MoTaNgan = "Khóa học lập trình web cho người mới",
                    UserId = 2,
                    DanhMucId = 1,
                    CapDo = "CoBan",
                    GiaTien = 0,
                    TrangThai = "DaXuatBan",
                    NgayTao = DateTime.Now
                },
                new KhoaHoc
                {
                    Id = 2,
                    MaKhoaHoc = "MOB001",
                    TenKhoaHoc = "Lập trình Android cơ bản",
                    MoTaNgan = "Học lập trình ứng dụng Android",
                    UserId = 3,
                    DanhMucId = 2,
                    CapDo = "CoBan",
                    GiaTien = 299000,
                    TrangThai = "DaXuatBan",
                    NgayTao = DateTime.Now
                }
            );

            
            modelBuilder.Entity<BaiHoc>().HasData(
      // Bài học cho khóa học WEB001 (HTML CSS JavaScript)
          new BaiHoc
          {
              Id = 1,
              KhoaHocId = 1,
              TenBaiHoc = "Giới thiệu về HTML",
              LoaiNoiDung = "Video",
              DuongDanNoiDung = "bai-1-gioi-thieu-html.mp4",
              ThuTuHienThi = 1
          },
          new BaiHoc
          {
              Id = 2,
              KhoaHocId = 1,
              TenBaiHoc = "Thẻ HTML cơ bản",
              LoaiNoiDung = "Video",
              DuongDanNoiDung = "bai-2-the-html-co-ban.mp4",
              ThuTuHienThi = 2
          },
          new BaiHoc
          {
              Id = 3,
              KhoaHocId = 1,
              TenBaiHoc = "Form và Input trong HTML",
              LoaiNoiDung = "Video",
              DuongDanNoiDung = "bai-3-form-input.mp4",
              ThuTuHienThi = 3
          },
          new BaiHoc
          {
              Id = 4,
              KhoaHocId = 1,
              TenBaiHoc = "CSS Selectors cơ bản",
              LoaiNoiDung = "Video",
              DuongDanNoiDung = "bai-4-css-selectors.mp4",
              ThuTuHienThi = 4
          },
          new BaiHoc
          {
              Id = 5,
              KhoaHocId = 1,
              TenBaiHoc = "Bài tập thực hành HTML CSS",
              LoaiNoiDung = "PDF",
              DuongDanNoiDung = "bai-5-bai-tap-thuc-hanh.pdf",
              ThuTuHienThi = 5
          },

          // Bài học cho khóa học MOB001 (Lập trình Android)
          new BaiHoc
          {
              Id = 6,
              KhoaHocId = 2,
              TenBaiHoc = "Giới thiệu Android Studio",
              LoaiNoiDung = "Video",
              DuongDanNoiDung = "bai-1-android-studio.mp4",
              ThuTuHienThi = 1
          },
          new BaiHoc
          {
              Id = 7,
              KhoaHocId = 2,
              TenBaiHoc = "Layout và View trong Android",
              LoaiNoiDung = "Video",
              DuongDanNoiDung = "bai-2-layout-view.mp4",
              ThuTuHienThi = 2
          },
          new BaiHoc
          {
              Id = 8,
              KhoaHocId = 2,
              TenBaiHoc = "Xử lý sự kiện click",
              LoaiNoiDung = "Video",
              DuongDanNoiDung = "bai-3-xu-ly-su-kien.mp4",
              ThuTuHienThi = 3
          },
          new BaiHoc
          {
              Id = 9,
              KhoaHocId = 2,
              TenBaiHoc = "Intent và Activity",
              LoaiNoiDung = "Video",
              DuongDanNoiDung = "bai-4-intent-activity.mp4",
              ThuTuHienThi = 4
          },
          new BaiHoc
          {
              Id = 10,
              KhoaHocId = 2,
              TenBaiHoc = "Dự án ứng dụng Calculator",
              LoaiNoiDung = "PDF",
              DuongDanNoiDung = "bai-5-du-an-calculator.pdf",
              ThuTuHienThi = 5
          }
      );

         
            modelBuilder.Entity<DangKyKhoaHoc>().HasData(
                new DangKyKhoaHoc
                {
                    Id = 1,
                    UserId = 3,
                    KhoaHocId = 1, 
                    NgayDangKy = DateTime.Now,
                    TrangThai = "DangHoc"
                },
                new DangKyKhoaHoc
                {
                    Id = 2,
                    UserId = 2, 
                    KhoaHocId = 2,
                    NgayDangKy = DateTime.Now,
                    TrangThai = "DangHoc"
                }
            );
        }
    }
    }

