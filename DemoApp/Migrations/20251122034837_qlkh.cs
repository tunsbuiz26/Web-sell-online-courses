using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApp.Migrations
{
    public partial class qlkh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMuc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThuTuHienThi = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMuc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NumberPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Cart_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KhoaHoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKhoaHoc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenKhoaHoc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MoTaNgan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnhBia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DanhMucId = table.Column<int>(type: "int", nullable: true),
                    CapDo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "CoBan"),
                    GiaTien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "BanNhap"),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhoaHoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KhoaHoc_DanhMuc_DanhMucId",
                        column: x => x.DanhMucId,
                        principalTable: "DanhMuc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_KhoaHoc_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaiHoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KhoaHocId = table.Column<int>(type: "int", nullable: false),
                    TenBaiHoc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LoaiNoiDung = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Video"),
                    DuongDanNoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThuTuHienThi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaiHoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaiHoc_KhoaHoc_KhoaHocId",
                        column: x => x.KhoaHocId,
                        principalTable: "KhoaHoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DangKyKhoaHoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    KhoaHocId = table.Column<int>(type: "int", nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "DangHoc")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKyKhoaHoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DangKyKhoaHoc_KhoaHoc_KhoaHocId",
                        column: x => x.KhoaHocId,
                        principalTable: "KhoaHoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DangKyKhoaHoc_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DanhMuc",
                columns: new[] { "Id", "MoTa", "TenDanhMuc", "ThuTuHienThi", "TrangThai" },
                values: new object[,]
                {
                    { 1, "Học phát triển website", "Lập Trình Web", 1, true },
                    { 2, "Phát triển ứng dụng di động", "Lập Trình Mobile", 2, true },
                    { 3, "Làm việc với Python", "Data Science", 3, true },
                    { 4, "UI (Giao diện người dùng), UX (Trải nghiệm người dùng)", "Thiết kế UX/UI Cơ Bản", 4, true }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "Email", "FullName", "NumberPhone", "Password", "RoleId", "Username" },
                values: new object[] { 1, "Hà Nội", "admin@khoahoc.vn", "Quản Trị Viên", "0123456789", "admin123", 1, "admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "Email", "FullName", "NumberPhone", "Password", "RoleId", "Username" },
                values: new object[] { 2, "TP HCM", "user01@khoahoc.vn", "Nguyễn Văn A", "0987654321", "user123", 2, "user01" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "Email", "FullName", "NumberPhone", "Password", "RoleId", "Username" },
                values: new object[] { 3, "Đà Nẵng", "user02@gmail.com", "Lê Văn C", "0912345678", "user321", 2, "user02" });

            migrationBuilder.InsertData(
                table: "KhoaHoc",
                columns: new[] { "Id", "AnhBia", "CapDo", "DanhMucId", "GiaTien", "MaKhoaHoc", "MoTaNgan", "NgayTao", "TenKhoaHoc", "TrangThai", "UserId" },
                values: new object[,]
                {
                    { 1, "anh1.jpg", "CoBan", 1, 399000m, "WEB001", "Khóa học lập trình web cho người mới", new DateTime(2025, 11, 22, 10, 48, 37, 97, DateTimeKind.Local).AddTicks(8689), "HTML CSS JavaScript Cơ Bản", "DaXuatBan", 1 },
                    { 2, "anh3.jpg", "CoBan", 2, 299000m, "MOB001", "Học lập trình ứng dụng Android", new DateTime(2025, 11, 22, 10, 48, 37, 97, DateTimeKind.Local).AddTicks(8691), "Lập trình Android cơ bản", "DaXuatBan", 3 },
                    { 3, "anh2.jpg", "TrungCap", 3, 450000m, "DATA001", "Khoa học dữ liệu với Python cơ bản", new DateTime(2025, 11, 17, 10, 48, 37, 97, DateTimeKind.Local).AddTicks(8693), "Python Data Science", "DaXuatBan", 2 },
                    { 4, "anh4.jpg", "CoBan", 4, 350000m, "UIUX001", "Học thiết kế giao diện người dùng chuyên nghiệp", new DateTime(2025, 11, 20, 10, 48, 37, 97, DateTimeKind.Local).AddTicks(8697), "Thiết kế UI/UX cơ bản", "BanNhap", 3 }
                });

            migrationBuilder.InsertData(
                table: "BaiHoc",
                columns: new[] { "Id", "DuongDanNoiDung", "KhoaHocId", "LoaiNoiDung", "TenBaiHoc", "ThuTuHienThi" },
                values: new object[,]
                {
                    { 1, "bai-1-gioi-thieu-web.mp4", 1, "Video", "Giới thiệu về Web Development", 1 },
                    { 2, "bai-2-cau-truc-html.mp4", 1, "Video", "Cấu trúc HTML cơ bản", 2 },
                    { 3, "bai-3-the-html-thong-dung.mp4", 1, "Video", "Các thẻ HTML thông dụng", 3 },
                    { 4, "bai-4-form-input.mp4", 1, "Video", "Form và Input trong HTML", 4 },
                    { 5, "bai-5-css-box-model.mp4", 1, "Video", "CSS Selectors và Box Model", 5 },
                    { 6, "bai-6-flexbox-grid.mp4", 1, "Video", "Flexbox và Grid Layout", 6 },
                    { 7, "bai-7-js-co-ban.mp4", 1, "Video", "JavaScript cơ bản - Variables & Functions", 7 },
                    { 8, "bai-8-dom-manipulation.mp4", 1, "Video", "DOM Manipulation", 8 },
                    { 9, "bai-9-du-an-portfolio.pdf", 1, "PDF", "Dự án Portfolio Website", 9 },
                    { 10, "bai-10-gioi-thieu-android.mp4", 2, "Video", "Giới thiệu Android Development", 1 },
                    { 11, "bai-11-cai-dat-android-studio.mp4", 2, "Video", "Cài đặt Android Studio", 2 },
                    { 12, "bai-12-layout-xml.mp4", 2, "Video", "Layout XML cơ bản", 3 },
                    { 13, "bai-13-layout-comparison.mp4", 2, "Video", "LinearLayout vs RelativeLayout", 4 },
                    { 14, "bai-14-xu-ly-su-kien.mp4", 2, "Video", "Xử lý sự kiện Click", 5 },
                    { 15, "bai-15-intent-navigation.mp4", 2, "Video", "Intent và Navigation", 6 },
                    { 16, "bai-16-recyclerview.mp4", 2, "Video", "RecyclerView cơ bản", 7 },
                    { 17, "bai-17-du-an-todo-list.pdf", 2, "PDF", "Dự án ứng dụng Todo List", 8 },
                    { 18, "bai-18-gioi-thieu-data-science.mp4", 3, "Video", "Giới thiệu Data Science", 1 },
                    { 19, "bai-19-cai-dat-moi-truong.mp4", 3, "Video", "Cài đặt môi trường Python & Jupyter", 2 },
                    { 20, "bai-20-pandas-dataframes.mp4", 3, "Video", "Pandas cơ bản - DataFrames", 3 },
                    { 21, "bai-21-data-cleaning.mp4", 3, "Video", "Data Cleaning với Pandas", 4 },
                    { 22, "bai-22-matplotlib.mp4", 3, "Video", "Data Visualization với Matplotlib", 5 },
                    { 23, "bai-23-seaborn.mp4", 3, "Video", "Seaborn cho visualization nâng cao", 6 },
                    { 24, "bai-24-phan-tich-du-lieu.pdf", 3, "PDF", "Phân tích dữ liệu thực tế", 7 },
                    { 25, "bai-25-gioi-thieu-ui-ux.mp4", 4, "Video", "Giới thiệu UI/UX Design", 1 },
                    { 26, "bai-26-nguyen-ly-thiet-ke.mp4", 4, "Video", "Nguyên lý thiết kế cơ bản", 2 },
                    { 27, "bai-27-color-theory.mp4", 4, "Video", "Color Theory trong UI Design", 3 },
                    { 28, "bai-28-typography.mp4", 4, "Video", "Typography cơ bản", 4 },
                    { 29, "bai-29-wireframing-figma.mp4", 4, "Video", "Wireframing với Figma", 5 },
                    { 30, "bai-30-prototyping.mp4", 4, "Video", "Prototyping và Interaction", 6 },
                    { 31, "bai-31-du-an-mobile-app.pdf", 4, "PDF", "Dự án thiết kế Mobile App", 7 },
                    { 32, "bai-32-design-system.mp4", 4, "Video", "Design System cơ bản", 8 }
                });

            migrationBuilder.InsertData(
                table: "DangKyKhoaHoc",
                columns: new[] { "Id", "KhoaHocId", "NgayDangKy", "TrangThai", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 11, 22, 10, 48, 37, 97, DateTimeKind.Local).AddTicks(8768), "DangHoc", 3 },
                    { 2, 2, new DateTime(2025, 11, 22, 10, 48, 37, 97, DateTimeKind.Local).AddTicks(8770), "DangHoc", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaiHoc_KhoaHocId",
                table: "BaiHoc",
                column: "KhoaHocId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserId",
                table: "Cart",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyKhoaHoc_KhoaHocId",
                table: "DangKyKhoaHoc",
                column: "KhoaHocId");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyKhoaHoc_UserId",
                table: "DangKyKhoaHoc",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyKhoaHoc_UserId_KhoaHocId",
                table: "DangKyKhoaHoc",
                columns: new[] { "UserId", "KhoaHocId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KhoaHoc_DanhMucId",
                table: "KhoaHoc",
                column: "DanhMucId");

            migrationBuilder.CreateIndex(
                name: "IX_KhoaHoc_MaKhoaHoc",
                table: "KhoaHoc",
                column: "MaKhoaHoc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KhoaHoc_TrangThai",
                table: "KhoaHoc",
                column: "TrangThai");

            migrationBuilder.CreateIndex(
                name: "IX_KhoaHoc_UserId",
                table: "KhoaHoc",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_RoleName",
                table: "Role",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaiHoc");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "DangKyKhoaHoc");

            migrationBuilder.DropTable(
                name: "KhoaHoc");

            migrationBuilder.DropTable(
                name: "DanhMuc");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
