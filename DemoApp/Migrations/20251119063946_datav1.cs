using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApp.Migrations
{
    public partial class datav1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DangKyKhoaHoc",
                keyColumn: "Id",
                keyValue: 1,
                column: "NgayDangKy",
                value: new DateTime(2025, 11, 19, 12, 59, 30, 339, DateTimeKind.Local).AddTicks(8404));

            migrationBuilder.UpdateData(
                table: "DangKyKhoaHoc",
                keyColumn: "Id",
                keyValue: 2,
                column: "NgayDangKy",
                value: new DateTime(2025, 11, 19, 12, 59, 30, 339, DateTimeKind.Local).AddTicks(8406));

            migrationBuilder.UpdateData(
                table: "KhoaHoc",
                keyColumn: "Id",
                keyValue: 1,
                column: "NgayTao",
                value: new DateTime(2025, 11, 19, 12, 59, 30, 339, DateTimeKind.Local).AddTicks(8367));

            migrationBuilder.UpdateData(
                table: "KhoaHoc",
                keyColumn: "Id",
                keyValue: 2,
                column: "NgayTao",
                value: new DateTime(2025, 11, 19, 12, 59, 30, 339, DateTimeKind.Local).AddTicks(8370));
        }
    }
}
