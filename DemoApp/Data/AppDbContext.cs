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
                entity.HasMany(e => e.KhoaHoc)
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
                    TenDanhMuc = "Data Science",
                    MoTa = "Làm việc với Python",
                    ThuTuHienThi = 3,
                    TrangThai = true
                },
                 new DanhMuc
                 {
                     Id = 4,
                     TenDanhMuc = "Thiết kế UX/UI Cơ Bản",
                     MoTa = "UI (Giao diện người dùng), UX (Trải nghiệm người dùng)",
                     ThuTuHienThi = 4,
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
                     AnhBia = "anh1.jpg",
                     UserId = 1,
                     DanhMucId = 1,
                     CapDo = "CoBan",
                     GiaTien = 399000,
                     TrangThai = "DaXuatBan",
                     NgayTao = DateTime.Now
                 },
                new KhoaHoc
                {
                    Id = 2,
                    MaKhoaHoc = "MOB001",
                    TenKhoaHoc = "Lập trình Android cơ bản",
                    MoTaNgan = "Học lập trình ứng dụng Android",
                    AnhBia = "anh3.jpg",
                    UserId = 3,
                    DanhMucId = 2,
                    CapDo = "CoBan",
                    GiaTien = 299000,
                    TrangThai = "DaXuatBan",
                    NgayTao = DateTime.Now
                },
                new KhoaHoc
                {
                    Id = 3,
                    MaKhoaHoc = "DATA001",
                    TenKhoaHoc = "Python Data Science",
                    MoTaNgan = "Khoa học dữ liệu với Python cơ bản",
                    AnhBia = "anh2.jpg",
                    UserId = 2,
                    DanhMucId = 3,
                    CapDo = "TrungCap",
                    GiaTien = 450000,
                    TrangThai = "DaXuatBan",
                    NgayTao = DateTime.Now.AddDays(-5)
                },
                new KhoaHoc
                {
                    Id = 4,
                    MaKhoaHoc = "UIUX001",
                    TenKhoaHoc = "Thiết kế UI/UX cơ bản",
                    MoTaNgan = "Học thiết kế giao diện người dùng chuyên nghiệp",
                    AnhBia = "anh4.jpg",
                    UserId = 3,
                    DanhMucId = 4,
                    CapDo = "CoBan",
                    GiaTien = 350000,
                    TrangThai = "BanNhap",
                    NgayTao = DateTime.Now.AddDays(-2)
                }
            );

            
            modelBuilder.Entity<BaiHoc>().HasData(
    // === BÀI HỌC CHO KHÓA HỌC 1: HTML CSS JavaScript Cơ Bản (WEB001) ===
    new BaiHoc
    {
        Id = 1,
        KhoaHocId = 1,
        TenBaiHoc = "Giới thiệu về Web Development",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-1-gioi-thieu-web.mp4",
        ThuTuHienThi = 1,
      
    },
    new BaiHoc
    {
        Id = 2,
        KhoaHocId = 1,
        TenBaiHoc = "Cấu trúc HTML cơ bản",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-2-cau-truc-html.mp4",
        ThuTuHienThi = 2,
    
    },
    new BaiHoc
    {
        Id = 3,
        KhoaHocId = 1,
        TenBaiHoc = "Các thẻ HTML thông dụng",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-3-the-html-thong-dung.mp4",
        ThuTuHienThi = 3,
       
    },
    new BaiHoc
    {
        Id = 4,
        KhoaHocId = 1,
        TenBaiHoc = "Form và Input trong HTML",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-4-form-input.mp4",
        ThuTuHienThi = 4,
     
    },
    new BaiHoc
    {
        Id = 5,
        KhoaHocId = 1,
        TenBaiHoc = "CSS Selectors và Box Model",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-5-css-box-model.mp4",
        ThuTuHienThi = 5,
    
    },
    new BaiHoc
    {
        Id = 6,
        KhoaHocId = 1,
        TenBaiHoc = "Flexbox và Grid Layout",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-6-flexbox-grid.mp4",
        ThuTuHienThi = 6,
      
    },
    new BaiHoc
    {
        Id = 7,
        KhoaHocId = 1,
        TenBaiHoc = "JavaScript cơ bản - Variables & Functions",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-7-js-co-ban.mp4",
        ThuTuHienThi = 7,
        
    },
    new BaiHoc
    {
        Id = 8,
        KhoaHocId = 1,
        TenBaiHoc = "DOM Manipulation",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-8-dom-manipulation.mp4",
        ThuTuHienThi = 8,
       
    },
    new BaiHoc
    {
        Id = 9,
        KhoaHocId = 1,
        TenBaiHoc = "Dự án Portfolio Website",
        LoaiNoiDung = "PDF",
        DuongDanNoiDung = "bai-9-du-an-portfolio.pdf",
        ThuTuHienThi = 9,
        
    },

    // === BÀI HỌC CHO KHÓA HỌC 2: Lập trình Android cơ bản (MOB001) ===
    new BaiHoc
    {
        Id = 10,
        KhoaHocId = 2,
        TenBaiHoc = "Giới thiệu Android Development",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-10-gioi-thieu-android.mp4",
        ThuTuHienThi = 1,
       
    },
    new BaiHoc
    {
        Id = 11,
        KhoaHocId = 2,
        TenBaiHoc = "Cài đặt Android Studio",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-11-cai-dat-android-studio.mp4",
        ThuTuHienThi = 2,
       
    },
    new BaiHoc
    {
        Id = 12,
        KhoaHocId = 2,
        TenBaiHoc = "Layout XML cơ bản",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-12-layout-xml.mp4",
        ThuTuHienThi = 3,
        
    },
    new BaiHoc
    {
        Id = 13,
        KhoaHocId = 2,
        TenBaiHoc = "LinearLayout vs RelativeLayout",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-13-layout-comparison.mp4",
        ThuTuHienThi = 4,
    }, 
    new BaiHoc
    {
        Id = 14,
        KhoaHocId = 2,
        TenBaiHoc = "Xử lý sự kiện Click",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-14-xu-ly-su-kien.mp4",
        ThuTuHienThi = 5,
      
    },
    new BaiHoc
    {
        Id = 15,
        KhoaHocId = 2,
        TenBaiHoc = "Intent và Navigation",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-15-intent-navigation.mp4",
        ThuTuHienThi = 6,
        
    },
    new BaiHoc
    {
        Id = 16,
        KhoaHocId = 2,
        TenBaiHoc = "RecyclerView cơ bản",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-16-recyclerview.mp4",
        ThuTuHienThi = 7,
       
    },
    new BaiHoc
    {
        Id = 17,
        KhoaHocId = 2,
        TenBaiHoc = "Dự án ứng dụng Todo List",
        LoaiNoiDung = "PDF",
        DuongDanNoiDung = "bai-17-du-an-todo-list.pdf",
        ThuTuHienThi = 8,
       
    },

    // === BÀI HỌC CHO KHÓA HỌC 3: Python Data Science (DATA001) ===
    new BaiHoc
    {
        Id = 18,
        KhoaHocId = 3,
        TenBaiHoc = "Giới thiệu Data Science",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-18-gioi-thieu-data-science.mp4",
        ThuTuHienThi = 1,
    
    },
    new BaiHoc
    {
        Id = 19,
        KhoaHocId = 3,
        TenBaiHoc = "Cài đặt môi trường Python & Jupyter",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-19-cai-dat-moi-truong.mp4",
        ThuTuHienThi = 2,
    
    },
    new BaiHoc
    {
        Id = 20,
        KhoaHocId = 3,
        TenBaiHoc = "Pandas cơ bản - DataFrames",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-20-pandas-dataframes.mp4",
        ThuTuHienThi = 3,
      
    },
    new BaiHoc
    {
        Id = 21,
        KhoaHocId = 3,
        TenBaiHoc = "Data Cleaning với Pandas",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-21-data-cleaning.mp4",
        ThuTuHienThi = 4,
        
    },
    new BaiHoc
    {
        Id = 22,
        KhoaHocId = 3,
        TenBaiHoc = "Data Visualization với Matplotlib",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-22-matplotlib.mp4",
        ThuTuHienThi = 5,
       
    },
    new BaiHoc
    {
        Id = 23,
        KhoaHocId = 3,
        TenBaiHoc = "Seaborn cho visualization nâng cao",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-23-seaborn.mp4",
        ThuTuHienThi = 6,

    },
    new BaiHoc
    {
        Id = 24,
        KhoaHocId = 3,
        TenBaiHoc = "Phân tích dữ liệu thực tế",
        LoaiNoiDung = "PDF",
        DuongDanNoiDung = "bai-24-phan-tich-du-lieu.pdf",
        ThuTuHienThi = 7,
      
    },

    // === BÀI HỌC CHO KHÓA HỌC 4: Thiết kế UI/UX cơ bản (UIUX001) ===
    new BaiHoc
    {
        Id = 25,
        KhoaHocId = 4,
        TenBaiHoc = "Giới thiệu UI/UX Design",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-25-gioi-thieu-ui-ux.mp4",
        ThuTuHienThi = 1,
      
    },
    new BaiHoc
    {
        Id = 26,
        KhoaHocId = 4,
        TenBaiHoc = "Nguyên lý thiết kế cơ bản",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-26-nguyen-ly-thiet-ke.mp4",
        ThuTuHienThi = 2,
     
    },
    new BaiHoc
    {
        Id = 27,
        KhoaHocId = 4,
        TenBaiHoc = "Color Theory trong UI Design",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-27-color-theory.mp4",
        ThuTuHienThi = 3,
        
    },
    new BaiHoc
    {
        Id = 28,
        KhoaHocId = 4,
        TenBaiHoc = "Typography cơ bản",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-28-typography.mp4",
        ThuTuHienThi = 4,
      
    },
    new BaiHoc
    {
        Id = 29,
        KhoaHocId = 4,
        TenBaiHoc = "Wireframing với Figma",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-29-wireframing-figma.mp4",
        ThuTuHienThi = 5,
      
    },
    new BaiHoc
    {
        Id = 30,
        KhoaHocId = 4,
        TenBaiHoc = "Prototyping và Interaction",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-30-prototyping.mp4",
        ThuTuHienThi = 6,
        
    },
    new BaiHoc
    {
        Id = 31,
        KhoaHocId = 4,
        TenBaiHoc = "Dự án thiết kế Mobile App",
        LoaiNoiDung = "PDF",
        DuongDanNoiDung = "bai-31-du-an-mobile-app.pdf",
        ThuTuHienThi = 7,
       
    },
    new BaiHoc
    {
        Id = 32,
        KhoaHocId = 4,
        TenBaiHoc = "Design System cơ bản",
        LoaiNoiDung = "Video",
        DuongDanNoiDung = "bai-32-design-system.mp4",
        ThuTuHienThi = 8,
        
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

