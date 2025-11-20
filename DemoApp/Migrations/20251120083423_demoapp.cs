using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApp.Migrations
{
    public partial class demoapp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DangKyKhoaHoc",
                keyColumn: "Id",
                keyValue: 1,
                column: "NgayDangKy",
                value: new DateTime(2025, 11, 20, 15, 34, 23, 376, DateTimeKind.Local).AddTicks(6200));

            migrationBuilder.UpdateData(
                table: "DangKyKhoaHoc",
                keyColumn: "Id",
                keyValue: 2,
                column: "NgayDangKy",
                value: new DateTime(2025, 11, 20, 15, 34, 23, 376, DateTimeKind.Local).AddTicks(6202));

            migrationBuilder.UpdateData(
                table: "KhoaHoc",
                keyColumn: "Id",
                keyValue: 1,
                column: "NgayTao",
                value: new DateTime(2025, 11, 20, 15, 34, 23, 376, DateTimeKind.Local).AddTicks(6128));

            migrationBuilder.UpdateData(
                table: "KhoaHoc",
                keyColumn: "Id",
                keyValue: 2,
                column: "NgayTao",
                value: new DateTime(2025, 11, 20, 15, 34, 23, 376, DateTimeKind.Local).AddTicks(6132));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DangKyKhoaHoc",
                keyColumn: "Id",
                keyValue: 1,
                column: "NgayDangKy",
                value: new DateTime(2025, 11, 19, 13, 39, 45, 701, DateTimeKind.Local).AddTicks(6341));

            migrationBuilder.UpdateData(
                table: "DangKyKhoaHoc",
                keyColumn: "Id",
                keyValue: 2,
                column: "NgayDangKy",
                value: new DateTime(2025, 11, 19, 13, 39, 45, 701, DateTimeKind.Local).AddTicks(6342));

            migrationBuilder.UpdateData(
                table: "KhoaHoc",
                keyColumn: "Id",
                keyValue: 1,
                column: "NgayTao",
                value: new DateTime(2025, 11, 19, 13, 39, 45, 701, DateTimeKind.Local).AddTicks(6286));

            migrationBuilder.UpdateData(
                table: "KhoaHoc",
                keyColumn: "Id",
                keyValue: 2,
                column: "NgayTao",
                value: new DateTime(2025, 11, 19, 13, 39, 45, 701, DateTimeKind.Local).AddTicks(6315));
        }
    }
}
