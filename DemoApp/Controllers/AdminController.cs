using System.Security.Claims;
using DemoApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
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
        public IActionResult Courses() => View();

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
