using System.Security.Claims;
using DemoApp.Data;
using DemoApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            // Lấy thông tin user từ claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var fullName = User.FindFirst("FullName")?.Value;

            // Truyền dữ liệu sang view nếu cần
            ViewData["AdminName"] = fullName ?? username;

            return View();
        }

        // Quản lý khóa học
        public IActionResult Courses()
        {
            var courses = _context.KhoaHoc
               .Include(k => k.DanhMuc)
               .Include(k => k.user)
               .Include(k => k.DangKyKhoaHoc)
               .ToList();

            ViewBag.DanhMucList = _context.DanhMuc.ToList();

            return View(courses);
        }

        // GET: /admin/courses/create  → Hiện form thêm khóa học
        [HttpGet("admin/courses/create")]
        public IActionResult CreateCourse()
        {
            // Danh mục để show dropdown
            ViewBag.DanhMucList = _context.DanhMuc.ToList();

            // Có thể set sẵn giá trị mặc định
            var model = new KhoaHoc
            {
                TrangThai = "BanNhap",
                CapDo = "CoBan",
                NgayTao = DateTime.Now
            };

            // View: Views/Admin/CreateCourse.cshtml
            return View("CreateCourse", model);
        }

        // POST: /admin/courses/create  → Nhận form, lưu DB
        [HttpPost("admin/courses/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(KhoaHoc model)
        {
            // Để khi ModelState invalid thì dropdown vẫn có dữ liệu
            ViewBag.DanhMucList = _context.DanhMuc.ToList();

            // Nếu chưa set UserId, gán theo user đang đăng nhập
            if (model.UserId == 0)
            {
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdStr, out int uid))
                {
                    model.UserId = uid;
                }
                else
                {
                    ModelState.AddModelError("", "Không lấy được thông tin giảng viên (UserId).");
                }
            }

            if (!ModelState.IsValid)
            {
                // Trả lại form với lỗi
                return View("CreateCourse", model);
            }

            model.NgayTao = DateTime.Now;

            _context.KhoaHoc.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thêm khóa học thành công!";

            // Quay lại list khóa học, lúc này sẽ thấy dòng mới thêm
            return RedirectToAction(nameof(Courses));
        }

        // Quản lý người dùng
        public IActionResult Users() => View();

        // Đơn hàng
        public IActionResult Orders() => View();

        // Doanh thu
        public IActionResult Revenue() => View();

        // Đánh giá
        public IActionResult Reviews() => View();
    }
}
