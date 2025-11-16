using DemoApp.Data;
using DemoApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Admins
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Trang Dashboard chính của Admin
        public IActionResult Index()
        {
            var model = new AdminDashboardViewModel
            {
                TongNguoiDung = _context.User.Count(),
                TongKhoaHoc = _context.KhoaHoc.Count(),
                TongBaiHoc = _context.BaiHoc.Count(),
                TongDangKy = _context.DangKyKhoaHoc.Count()
            };

            return View(model);
        }

       
        public async Task<IActionResult> NguoiDung()
        {
            var users = await _context.User.ToListAsync();
            return View(users);
        }

        
        public async Task<IActionResult> KhoaHoc()
        {
            var khoahoc = await _context.KhoaHoc.ToListAsync();
            return View(khoahoc);
        }

       
        public async Task<IActionResult> BaiHoc()
        {
            var baihoc = await _context.BaiHoc
                .Include(x => x.KhoaHoc)
                .ToListAsync();
            return View(baihoc);
        }

       
        public async Task<IActionResult> DangKyKhoaHoc()
        {
            var dk = await _context.DangKyKhoaHoc
                .Include(x => x.User)
                .Include(x => x.KhoaHoc)
                .ToListAsync();

            return View(dk);
        }
    }
}
