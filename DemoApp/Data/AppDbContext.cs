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
        public DbSet<DemoApp.Models.BaiHoc> BaiHoc { get; set; } = default!;
        public DbSet<DemoApp.Models.KhoaHoc> KhoaHoc { get; set; } = default!;
        public DbSet<DemoApp.Models.DangKyKhoaHoc> DangKyKhoaHoc { get; set; } = default!;
        public DbSet<DemoApp.Models.TienDoHocTap> TienDoHocTap { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DangKyKhoaHoc>()
                .HasKey(dk => new { dk.UserId, dk.KhoaHocID });

            modelBuilder.Entity<DangKyKhoaHoc>()
                .HasOne(dk => dk.User)
                .WithMany(nd => nd.dangKyKhoaHocs)
                .HasForeignKey(dk => dk.UserId);

            modelBuilder.Entity<DangKyKhoaHoc>()
                .HasOne(dk => dk.KhoaHoc)
                .WithMany(nd => nd.dangKyKhoaHocs)
                .HasForeignKey(dk => dk.KhoaHocID);

            modelBuilder.Entity<TienDoHocTap>()
                .HasKey(dk => new { dk.UserId, dk.BaiHocID });

            modelBuilder.Entity<TienDoHocTap>()
                .HasOne(dk => dk.User)
                .WithMany(nd => nd.tienDoHoctaps)
                .HasForeignKey(dk => dk.UserId);

            modelBuilder.Entity<TienDoHocTap>()
               .HasOne(dk => dk.BaiHoc)
               .WithMany(nd => nd.tienDoHoctaps)
               .HasForeignKey(dk => dk.BaiHocID);

        }
    }
}
