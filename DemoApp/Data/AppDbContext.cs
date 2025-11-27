using System;
using Microsoft.EntityFrameworkCore;
using DemoApp.Models;

namespace DemoApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<KhoaHoc> KhoaHoc { get; set; } = default!;
        public DbSet<BaiHoc> BaiHoc { get; set; } = default!;
        public DbSet<DangKyKhoaHoc> DangKyKhoaHoc { get; set; } = default!;
        public DbSet<DanhMuc> DanhMuc { get; set; } = default!;
        public DbSet<Role> Role { get; set; } = default!;
        public DbSet<TienDoHocTap> TienDoHocTap { get; set; } = default!;
        public DbSet<Cart> Cart { get; set; } = default!;
        public DbSet<BuoiHoc> BuoiHoc { get; set; } = default!;
        public DbSet<DiemDanh> DiemDanh { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== User =====
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.RoleId);

                entity.HasMany(u => u.KhoaHoc)
                      .WithOne(kh => kh.user)
                      .HasForeignKey(kh => kh.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.TienDoHocTap)
                      .WithOne(td => td.User)
                      .HasForeignKey(td => td.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.DangKyKhoaHocs)
                      .WithOne(dk => dk.user)
                      .HasForeignKey(dk => dk.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.Carts)
                      .WithOne(c => c.User)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany<DiemDanh>()
                      .WithOne(dd => dd.User!)
                      .HasForeignKey(dd => dd.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== Role =====
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.RoleName).IsUnique();
            });

            // ===== DanhMuc =====
            modelBuilder.Entity<DanhMuc>(entity =>
            {
                entity.Property(e => e.TrangThai).HasDefaultValue(true);

                entity.HasMany(dm => dm.KhoaHoc)
                      .WithOne(kh => kh.DanhMuc)
                      .HasForeignKey(kh => kh.DanhMucId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== KhoaHoc =====
            modelBuilder.Entity<KhoaHoc>(entity =>
            {
                entity.HasIndex(e => e.MaKhoaHoc).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.DanhMucId);
                entity.HasIndex(e => e.TrangThai);

                entity.Property(e => e.CapDo).HasDefaultValue("CoBan");
                entity.Property(e => e.TrangThai).HasDefaultValue("BanNhap");
                entity.Property(e => e.GiaTien).HasPrecision(18, 2).HasDefaultValue(0);

                entity.HasMany(kh => kh.TienDoHocTap)
                      .WithOne(td => td.KhoaHoc)
                      .HasForeignKey(td => td.KhoaHocId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(kh => kh.BaiHoc)
                      .WithOne(bh => bh.KhoaHoc)
                      .HasForeignKey(bh => bh.KhoaHocId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(kh => kh.DangKyKhoaHoc)
                      .WithOne(dk => dk.KhoaHoc)
                      .HasForeignKey(dk => dk.KhoaHocId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(kh => kh.user)
                      .WithMany(u => u.KhoaHoc)
                      .HasForeignKey(kh => kh.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(kh => kh.DanhMuc)
                      .WithMany(dm => dm.KhoaHoc)
                      .HasForeignKey(kh => kh.DanhMucId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany<BuoiHoc>()
                      .WithOne(b => b.KhoaHoc!)
                      .HasForeignKey(b => b.KhoaHocId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany<DiemDanh>()
                      .WithOne(dd => dd.KhoaHoc!)
                      .HasForeignKey(dd => dd.KhoaHocId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== BaiHoc =====
            modelBuilder.Entity<BaiHoc>(entity =>
            {
                entity.HasIndex(e => e.KhoaHocId);
                entity.Property(e => e.LoaiNoiDung).HasDefaultValue("Video");

                entity.HasMany(bh => bh.TienDoHocTap)
                      .WithOne(td => td.BaiHoc)
                      .HasForeignKey(td => td.BaiHocId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bh => bh.KhoaHoc)
                      .WithMany(kh => kh.BaiHoc)
                      .HasForeignKey(bh => bh.KhoaHocId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== DangKyKhoaHoc =====
            modelBuilder.Entity<DangKyKhoaHoc>(entity =>
            {
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.KhoaHocId);
                entity.HasIndex(e => new { e.UserId, e.KhoaHocId }).IsUnique();

                entity.Property(e => e.TrangThai)
                      .HasMaxLength(20)
                      .HasDefaultValue("DangHoc");

                entity.HasOne(dk => dk.user)
                      .WithMany(u => u.DangKyKhoaHocs)
                      .HasForeignKey(dk => dk.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(dk => dk.KhoaHoc)
                      .WithMany(kh => kh.DangKyKhoaHoc)
                      .HasForeignKey(dk => dk.KhoaHocId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== TienDoHocTap =====
            modelBuilder.Entity<TienDoHocTap>(entity =>
            {
                entity.HasOne(td => td.User)
                      .WithMany(u => u.TienDoHocTap)
                      .HasForeignKey(td => td.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(td => td.KhoaHoc)
                      .WithMany(kh => kh.TienDoHocTap)
                      .HasForeignKey(td => td.KhoaHocId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(td => td.BaiHoc)
                      .WithMany(bh => bh.TienDoHocTap)
                      .HasForeignKey(td => td.BaiHocId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== Cart =====
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.CartId);
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Carts)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== BuoiHoc =====
            modelBuilder.Entity<BuoiHoc>(entity =>
            {
                entity.HasIndex(e => e.KhoaHocId);

                entity.HasOne(bh => bh.KhoaHoc)
                      .WithMany(kh => kh.BuoiHocs!)
                      .HasForeignKey(bh => bh.KhoaHocId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(bh => bh.DiemDanhs!)
                      .WithOne(dd => dd.BuoiHoc!)
                      .HasForeignKey(dd => dd.BuoiHocId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== DiemDanh =====
            modelBuilder.Entity<DiemDanh>(entity =>
            {
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.KhoaHocId);
                entity.HasIndex(e => e.BuoiHocId);

                entity.Property(e => e.TrangThai)
                      .HasMaxLength(20)
                      .HasDefaultValue("present");

                entity.HasOne(dd => dd.User)
                      .WithMany(u => u.DiemDanhs!)
                      .HasForeignKey(dd => dd.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(dd => dd.KhoaHoc)
                      .WithMany(kh => kh.DiemDanhs!)
                      .HasForeignKey(dd => dd.KhoaHocId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(dd => dd.BuoiHoc)
                      .WithMany(bh => bh.DiemDanhs!)
                      .HasForeignKey(dd => dd.BuoiHocId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // ===== Role =====
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "User" }
            );

            // ===== User =====
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

            // ===== DanhMuc ===== (giữ tạm, dùng lại cho 4 khóa)
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
                    TenDanhMuc = "Cơ Sở Dữ Liệu & Backend",
                    MoTa = "Cơ sở dữ liệu, SQL, backend",
                    ThuTuHienThi = 3,
                    TrangThai = true
                },
                new DanhMuc
                {
                    Id = 4,
                    TenDanhMuc = "Frontend Framework",
                    MoTa = "React, Vue, Angular...",
                    ThuTuHienThi = 4,
                    TrangThai = true
                }
            );

            // ===== KhoaHoc (4 khóa theo yêu cầu) =====
            modelBuilder.Entity<KhoaHoc>().HasData(
<<<<<<< HEAD
                new KhoaHoc
                {
                    Id = 1,
                    MaKhoaHoc = "CPP001",
                    TenKhoaHoc = "Lập trình C++",
                    MoTaNgan = "Khóa học lập trình C++ cho người mới",
                    AnhBia = "cpp.png",
                    UserId = 1,
                    DanhMucId = 3, // tạm xếp vào CSDL/Backend
                    CapDo = "CoBan",
                    GiaTien = 150000,
                    TrangThai = "DaXuatBan",
                    NgayTao = DateTime.Now
                },
                new KhoaHoc
                {
                    Id = 2,
                    MaKhoaHoc = "HTMLCSS001",
                    TenKhoaHoc = "Lập trình HTML và CSS",
                    MoTaNgan = "Khóa học HTML CSS cơ bản",
                    AnhBia = "htmlcss.jpg",
                    UserId = 1,
                    DanhMucId = 1,
=======
                 new KhoaHoc
                 {
                     Id = 1,
                     MaKhoaHoc = "WEB001",
                     TenKhoaHoc = "HTML CSS JavaScript Cơ Bản",
                     MoTaNgan = "Khóa học lập trình web cho người mới",
                     MoTaChiTiet = "Biến bạn từ người mới bắt đầu thành người xây dựng website chuyên nghiệp!" +
                     "Khóa học HTML CSS JavaScript Cơ Bản chính là nền tảng vững chắc đầu tiên dành cho bạn!" +
                     "Khóa học được thiết kế từ CƠ BẢN đến NÂNG CAO, phù hợp cho người mới bắt đầu. Bạn chỉ cần: Chăm chỉ và kiên trì thực hành,không ngại hỏi đáp, tích cực tương tác,chủ động đưa ra thắc mắc trong quá trình học tập" +
                     "Sau khóa học, bạn ĐỦ NĂNG LỰC để: Ứng tuyển vị trí Fresher Frontend Developer, tự tin xây dựng website responsive, phát triển các dự án cá nhân chất lượng" +
                     "ĐĂNG KÝ NGAY ĐỂ BIẾN ĐAM MÊ THÀNH NGHỀ NGHIỆP!",
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
                    MoTaChiTiet = "Biến ý tưởng thành ứng dụng thực tế - Khởi đầu sự nghiệp Mobile Developer!" +
                    "Khóa học Lập trình Android Cơ Bản chính là chìa khóa mở cánh cửa vào thế giới phát triển di động!" +
                    "Khóa học được thiết kế từ CƠ BẢN đến THỰC HÀNH, chỉ cần bạn: Chăm chỉ thực hành và không ngại thử nghiệm, tích cực hỏi đáp và chia sẻ khó khăn, có laptop cấu hình tối thiểu để cài đặt Android Studio" +
                    "Sau khóa học, bạn HOÀN TOÀN ĐỦ NĂNG LỰC để: Ứng tuyển vị trí Fresher Android Developer, tự phát triển ứng dụng Android hoàn chỉnh, hiểu và áp dụng Android architecture patterns, kết nối với RESTful API và xử lý data public ứng dụng lên Google Play Store" +
                    "ĐĂNG KÝ NGAY ĐỂ BIẾN ĐAM MÊ THÀNH NGHỀ NGHIỆP!",
                    AnhBia = "anh3.jpg",
                    UserId = 3,
                    DanhMucId = 2,
>>>>>>> 1ddccca520d1f33fe634b9d6b6092e8d45c32975
                    CapDo = "CoBan",
                    GiaTien = 300000,
                    TrangThai = "DaXuatBan",
                    NgayTao = DateTime.Now
                },
                new KhoaHoc
                {
                    Id = 3,
<<<<<<< HEAD
                    MaKhoaHoc = "SQL001",
                    TenKhoaHoc = "Lập trình SQL",
                    MoTaNgan = "SQL cho người mới bắt đầu",
                    AnhBia = "sql.png",
                    UserId = 1,
=======
                    MaKhoaHoc = "DATA001",
                    TenKhoaHoc = "Python Data Science",
                    MoTaNgan = "Khoa học dữ liệu với Python cơ bản",
                    MoTaChiTiet = "Biến dữ liệu thô thành insights giá trị - Khởi đầu sự nghiệp Data Scientist!" +
                    "Khóa học Python Data Science sẽ trang bị cho bạn công cụ để trở thành chuyên gia phân tích dữ liệu thời đại 4.0!" +
                    "Khóa học được thiết kế từ CƠ BẢN đến CHUYÊN SÂU, chỉ cần bạn: Tư duy logic và ham học hỏi, chăm chỉ thực hành case study thực tế, không ngại hỏi đáp về các vấn đề data" +
                    "Sau khóa học, bạn HOÀN TOÀN có thể: Ứng tuyển thành công vị trí Fresher Data Analyst, tự tin phỏng vấn với technical knowledge, xử lý real-world data problems, tiếp tục học lên Advanced Data Science" +
                    "ĐĂNG KÝ NGAY ĐỂ BIẾN ĐAM MÊ THÀNH NGHỀ NGHIỆP!",
                    AnhBia = "anh2.jpg",
                    UserId = 2,
>>>>>>> 1ddccca520d1f33fe634b9d6b6092e8d45c32975
                    DanhMucId = 3,
                    CapDo = "CoBan",
                    GiaTien = 250000,
                    TrangThai = "DaXuatBan",
                    NgayTao = DateTime.Now
                },
                new KhoaHoc
                {
                    Id = 4,
<<<<<<< HEAD
                    MaKhoaHoc = "REACT001",
                    TenKhoaHoc = "Lập trình React",
                    MoTaNgan = "Khóa học ReactJS cơ bản",
                    AnhBia = "react.png",
                    UserId = 1,
=======
                    MaKhoaHoc = "UIUX001",
                    TenKhoaHoc = "Thiết kế UI/UX cơ bản",
                    MoTaNgan = "Học thiết kế giao diện người dùng chuyên nghiệp",
                    MoTaChiTiet = "Biến ý tưởng thành trải nghiệm người dùng tuyệt vời - Khởi đầu sự nghiệp Designer chuyên nghiệp!" +
                    "Khóa học Thiết kế UI/UX Cơ Bản sẽ biến bạn từ người mới bắt đầu thành designer có tư duy hệ thống!" +
                    "Khóa học được thiết kế từ NGUYÊN LÝ đến THỰC HÀNH, chỉ cần bạn: Đam mê sáng tạo và yêu thích cái đẹp, tư duy logic và hướng về người dùng, chăm chỉ thực hành và không ngại feedback" +
                    "Sau khóa học, bạn HOÀN TOÀN có thể: Ứng tuyển thành công vị trí Fresher UI/UX Designer, làm freelance project với mức lương 2-5 triệu/project, tiếp tục học nâng cao lên Senior level, tự quản lý design process từ A-Z" +
                    "ĐĂNG KÝ NGAY ĐỂ BIẾN ĐAM MÊ THÀNH NGHỀ NGHIỆP!",
                    AnhBia = "anh4.jpg",
                    UserId = 3,
>>>>>>> 1ddccca520d1f33fe634b9d6b6092e8d45c32975
                    DanhMucId = 4,
                    CapDo = "CoBan",
                    GiaTien = 500000,
                    TrangThai = "DaXuatBan",
                    NgayTao = DateTime.Now
                }
            );

            // ===== BaiHoc (YouTube URL) =====
            modelBuilder.Entity<BaiHoc>().HasData(
                // 1. Lập trình C++
                new BaiHoc
                {
                    Id = 1,
                    KhoaHocId = 1,
                    TenBaiHoc = "Lộ Trình Học Lập Trình Cho Người Mới",
                    LoaiNoiDung = "Video",
                    DuongDanNoiDung = "https://www.youtube.com/watch?v=S3nx34WFXjI&list=PLZPZq0r_RZOMHoXIcxze_lP97j2Ase2on",
                    ThuTuHienThi = 1,
                    ThoiLuong = 0
                },
                new BaiHoc
                {
                    Id = 2,
                    KhoaHocId = 1,
                    TenBaiHoc = "Làm Quen Với Ngôn Ngữ Lập Trình C++",
                    LoaiNoiDung = "Video",
                    DuongDanNoiDung = "https://www.youtube.com/watch?v=74B6PXO97Tw&list=PLux-_phi0Rz0Hq9fDP4TlOulBl8APKp79&index=2",
                    ThuTuHienThi = 2,
                    ThoiLuong = 0
                },
                new BaiHoc
                {
                    Id = 3,
                    KhoaHocId = 1,
                    TenBaiHoc = "Toàn Tập Về Các Toán Tử Cơ Bản Trong Ngôn Ngữ Lập Trình C++ (Phần 1)",
                    LoaiNoiDung = "Video",
                    DuongDanNoiDung = "https://www.youtube.com/watch?v=y-_fNgvSfjc&list=PLux-_phi0Rz0Hq9fDP4TlOulBl8APKp79&index=3",
                    ThuTuHienThi = 3,
                    ThoiLuong = 0
                },
                new BaiHoc
                {
                    Id = 4,
                    KhoaHocId = 1,
                    TenBaiHoc = "Toàn Tập Về Các Toán Tử Cơ Bản Trong Ngôn Ngữ Lập Trình C++ (Phần 2)",
                    LoaiNoiDung = "Video",
                    DuongDanNoiDung = "https://www.youtube.com/watch?v=y-_fNgvSfjc&list=PLux-_phi0Rz0Hq9fDP4TlOulBl8APKp79&index=3",
                    ThuTuHienThi = 4,
                    ThoiLuong = 0
                },

                // 2. Lập trình HTML và CSS
                new BaiHoc
                {
                    Id = 5,
                    KhoaHocId = 2,
                    TenBaiHoc = "Tổng quan về khóa học HTML CSS",
                    LoaiNoiDung = "Video",
                    DuongDanNoiDung = "https://www.youtube.com/watch?v=R6plN3FvzFY&list=PL_-VfJajZj0U9nEXa4qyfB4U5ZIYCMPlz",
                    ThuTuHienThi = 1,
                    ThoiLuong = 0
                },
                new BaiHoc
                {
                    Id = 6,
                    KhoaHocId = 2,
                    TenBaiHoc = "HTML CSS là gì? | Ví dụ trực quan về HTML & CSS",
                    LoaiNoiDung = "Video",
                    DuongDanNoiDung = "http://youtube.com/watch?v=zwsPND378OQ&list=PL_-VfJajZj0U9nEXa4qyfB4U5ZIYCMPlz&index=2",
                    ThuTuHienThi = 2,
                    ThoiLuong = 0
                },
                new BaiHoc
                {
                    Id = 7,
                    KhoaHocId = 2,
                    TenBaiHoc = "Làm quen với Dev tools",
                    LoaiNoiDung = "Video",
                    DuongDanNoiDung = "https://www.youtube.com/watch?v=7BJiPyN4zZ0&list=PL_-VfJajZj0U9nEXa4qyfB4U5ZIYCMPlz&index=3",
                    ThuTuHienThi = 3,
                    ThoiLuong = 0
                },

                // 3. Lập trình SQL
                new BaiHoc
                {
                    Id = 8,
                    KhoaHocId = 3,
                    TenBaiHoc = "SQL cho người mới bắt đầu",
                    LoaiNoiDung = "Video",
                    DuongDanNoiDung = "https://www.youtube.com/watch?v=oPV2sjMG53U&list=PLZPZq0r_RZOMskz6MdsMOgxzheIyjo-BZ",
                    ThuTuHienThi = 1,
                    ThoiLuong = 0
                },

                // 4. Lập trình React
                new BaiHoc
                {
                    Id = 9,
                    KhoaHocId = 4,
                    TenBaiHoc = "ReactJS là gì | Tại sao nên học ReactJS | Khóa học ReactJS miễn phí",
                    LoaiNoiDung = "Video",
                    DuongDanNoiDung = "https://www.youtube.com/watch?v=x0fSBAgBrOQ&list=PL_-VfJajZj0UXjlKfBwFX73usByw3Ph9Q",
                    ThuTuHienThi = 1,
                    ThoiLuong = 0
                }
            );
            // ===== DangKyKhoaHoc: user 2 đăng ký khóa C++ (KhoaHocId = 1) =====
            modelBuilder.Entity<DangKyKhoaHoc>().HasData(
                new DangKyKhoaHoc
                {
                    Id = 1,
                    UserId = 2,       // user01@khoahoc.vn
                    KhoaHocId = 1,    // Lập trình C++
                    NgayDangKy = DateTime.Now.AddDays(-3),
                    TrangThai = "DangHoc"
                }
            );
            // ===== BuoiHoc: 2 buổi học cho khóa C++ =====
            modelBuilder.Entity<BuoiHoc>().HasData(
                new BuoiHoc
                {
                    Id = 1,
                    KhoaHocId = 1,
                    TenBuoiHoc = "Buổi 1 - Lộ trình học & bài 1 C++",
                    NgayHoc = DateTime.Today.AddDays(-3),
                    ThoiLuong = 60,      // phút (ví dụ)
                    ThuTuBuoi = 1
                },
                new BuoiHoc
                {
                    Id = 2,
                    KhoaHocId = 1,
                    TenBuoiHoc = "Buổi 2 - Làm quen với C++",
                    NgayHoc = DateTime.Today.AddDays(-2),
                    ThoiLuong = 60,      // phút (ví dụ)
                    ThuTuBuoi = 2
                }
            );
            // ===== DiemDanh: user 2 điểm danh khóa C++ =====
            modelBuilder.Entity<DiemDanh>().HasData(
                new DiemDanh
                {
                    Id = 1,
                    UserId = 2,
                    KhoaHocId = 1,
                    BuoiHocId = 1,
                    NgayDiemDanh = DateTime.Today.AddDays(-3),
                    TrangThai = "present",
                    GhiChu = "Có mặt đầy đủ buổi 1"
                },
                new DiemDanh
                {
                    Id = 2,
                    UserId = 2,
                    KhoaHocId = 1,
                    BuoiHocId = 2,
                    NgayDiemDanh = DateTime.Today.AddDays(-2),
                    TrangThai = "late",
                    GhiChu = "Đến muộn 5 phút buổi 2"
                }
            );
            // ===== TienDoHocTap: user 2 học các bài trong khóa C++ =====
            modelBuilder.Entity<TienDoHocTap>().HasData(
                new TienDoHocTap
                {
                    Id = 1,
                    UserId = 2,
                    KhoaHocId = 1,
                    BaiHocId = 1,                 // Bài 1 - Lộ trình học lập trình
                    DaHoanThanh = true,
                    ThoiGianBatDau = DateTime.Now.AddDays(-2).AddMinutes(-30),
                    ThoiGianHoanThanh = DateTime.Now.AddDays(-2),
                    ThoiGianCapNhat = DateTime.Now.AddDays(-2),
                    TyLeHoanThanh = 100m,         // 100%
                    ThoiGianHoc = 30,             // ví dụ ~30 phút (gần đúng thời lượng video)
                    TrangThaiHoc = "DaHoanThanh"
                },
                new TienDoHocTap
                {
                    Id = 2,
                    UserId = 2,
                    KhoaHocId = 1,
                    BaiHocId = 2,                 // Bài 2 - Làm quen với C++
                    DaHoanThanh = false,
                    ThoiGianBatDau = DateTime.Now.AddDays(-1).AddMinutes(-10),
                    ThoiGianHoanThanh = null,
                    ThoiGianCapNhat = DateTime.Now.AddDays(-1),
                    TyLeHoanThanh = 40m,          // mới xem khoảng 40%
                    ThoiGianHoc = 10,             // ví dụ đã học 10 phút
                    TrangThaiHoc = "DangHoc"
                },
                new TienDoHocTap
                {
                    Id = 3,
                    UserId = 2,
                    KhoaHocId = 1,
                    BaiHocId = 3,                 // Bài 3 - Toán tử cơ bản C++
                    DaHoanThanh = false,
                    ThoiGianBatDau = null,
                    ThoiGianHoanThanh = null,
                    ThoiGianCapNhat = DateTime.Now,
                    TyLeHoanThanh = 0m,
                    ThoiGianHoc = 0,
                    TrangThaiHoc = "DangHoc"
                }
            );
        }
    }
}
