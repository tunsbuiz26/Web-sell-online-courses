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
                    { 3, "SQL Server, MySQL", "Cơ Sở Dữ Liệu", 3, true }
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
                columns: new[] { "Id", "AnhBia", "CapDo", "DanhMucId", "MaKhoaHoc", "MoTaNgan", "NgayTao", "TenKhoaHoc", "TrangThai", "UserId" },
                values: new object[] { 1, null, "CoBan", 1, "WEB001", "Khóa học lập trình web cho người mới", new DateTime(2025, 11, 19, 12, 59, 30, 339, DateTimeKind.Local).AddTicks(8367), "HTML CSS JavaScript Cơ Bản", "DaXuatBan", 2 });

            migrationBuilder.InsertData(
                table: "KhoaHoc",
                columns: new[] { "Id", "AnhBia", "CapDo", "DanhMucId", "GiaTien", "MaKhoaHoc", "MoTaNgan", "NgayTao", "TenKhoaHoc", "TrangThai", "UserId" },
                values: new object[] { 2, null, "CoBan", 2, 299000m, "MOB001", "Học lập trình ứng dụng Android", new DateTime(2025, 11, 19, 12, 59, 30, 339, DateTimeKind.Local).AddTicks(8370), "Lập trình Android cơ bản", "DaXuatBan", 3 });

            migrationBuilder.InsertData(
                table: "BaiHoc",
                columns: new[] { "Id", "DuongDanNoiDung", "KhoaHocId", "LoaiNoiDung", "TenBaiHoc", "ThuTuHienThi" },
                values: new object[,]
                {
                    { 1, "bai-1-gioi-thieu-html.mp4", 1, "Video", "Giới thiệu về HTML", 1 },
                    { 2, "bai-2-the-html-co-ban.mp4", 1, "Video", "Thẻ HTML cơ bản", 2 },
                    { 3, "bai-3-form-input.mp4", 1, "Video", "Form và Input trong HTML", 3 },
                    { 4, "bai-4-css-selectors.mp4", 1, "Video", "CSS Selectors cơ bản", 4 },
                    { 5, "bai-5-bai-tap-thuc-hanh.pdf", 1, "PDF", "Bài tập thực hành HTML CSS", 5 },
                    { 6, "bai-1-android-studio.mp4", 2, "Video", "Giới thiệu Android Studio", 1 },
                    { 7, "bai-2-layout-view.mp4", 2, "Video", "Layout và View trong Android", 2 },
                    { 8, "bai-3-xu-ly-su-kien.mp4", 2, "Video", "Xử lý sự kiện click", 3 },
                    { 9, "bai-4-intent-activity.mp4", 2, "Video", "Intent và Activity", 4 },
                    { 10, "bai-5-du-an-calculator.pdf", 2, "PDF", "Dự án ứng dụng Calculator", 5 }
                });

            migrationBuilder.InsertData(
                table: "DangKyKhoaHoc",
                columns: new[] { "Id", "KhoaHocId", "NgayDangKy", "TrangThai", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 11, 19, 12, 59, 30, 339, DateTimeKind.Local).AddTicks(8404), "DangHoc", 3 },
                    { 2, 2, new DateTime(2025, 11, 19, 12, 59, 30, 339, DateTimeKind.Local).AddTicks(8406), "DangHoc", 2 }
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
