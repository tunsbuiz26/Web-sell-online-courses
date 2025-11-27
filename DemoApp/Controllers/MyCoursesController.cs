using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DemoApp.Data;
using DemoApp.Models;
using DemoApp.ViewModels;

namespace DemoApp.Controllers
{
    [Authorize] // bắt buộc đăng nhập mới xem được khóa học của tôi
    public class MyCoursesController : Controller
    {
        private readonly AppDbContext _context;

        public MyCoursesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Lấy thông tin user hiện tại
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var identityName = User.Identity.Name; // thường là Email hoặc Username

            var currentUser = await _context.User
                .FirstOrDefaultAsync(u =>
                    u.Email == identityName || u.Username == identityName);

            if (currentUser == null)
            {
                // không tìm được user trong DB -> bắt login lại
                return RedirectToAction("Login", "Account");
            }

            // 2. Lấy danh sách đăng ký khóa học của user
            var registrations = await _context.DangKyKhoaHoc
                .Where(dk => dk.UserId == currentUser.UserId
                             && dk.TrangThai == "DangHoc") // chỉ lấy khóa đang học
                .Include(dk => dk.KhoaHoc!)
                    .ThenInclude(kh => kh.BaiHoc)
                .ToListAsync();

            // 3. Lấy tiến độ học tập của user cho tất cả khóa
            var tienDoList = await _context.TienDoHocTap
                .Where(td => td.UserId == currentUser.UserId)
                .ToListAsync();

            // 4. Map sang MyCourseItemViewModel
            var viewModel = new List<MyCourseItemViewModel>();

            foreach (var dk in registrations)
            {
                var kh = dk.KhoaHoc!;
                var totalLessons = kh.BaiHoc?.Count ?? 0;

                // Đếm số bài đã hoàn thành trong TienDoHocTap
                var completedLessons = tienDoList
                    .Where(td => td.KhoaHocId == kh.Id && td.DaHoanThanh)
                    .Select(td => td.BaiHocId)
                    .Distinct()
                    .Count();

                int progressPercent = 0;
                if (totalLessons > 0)
                {
                    progressPercent = (int)Math.Round(
                        completedLessons * 100.0 / totalLessons
                    );
                }

                viewModel.Add(new MyCourseItemViewModel
                {
                    CourseId = kh.Id,
                    CourseTitle = kh.TenKhoaHoc,
                    ShortDescription = kh.MoTaNgan,
                    ThumbnailUrl = "/images/" + (kh.AnhBia ?? "default-course.jpg"),
                    Level = MapLevel(kh.CapDo),
                    TotalLessons = totalLessons,
                    ProgressPercent = progressPercent,
                    StartDate = dk.NgayDangKy
                });
            }

            // 5. Trả về view /Views/MyCourses/Index.cshtml với Model là List<MyCourseItemViewModel>
            return View(viewModel);
        }

        // Hàm phụ để convert CapDo -> text đẹp
        private string MapLevel(string? capDo)
        {
            return capDo switch
            {
                "CoBan" => "Cơ bản",
                "TrungCap" => "Trung cấp",
                "NangCao" => "Nâng cao",
                _ => "All level"
            };
        }
        // ================== CHI TIẾT MỘT KHÓA HỌC ==================
        public async Task<IActionResult> Details(int id, int? activeLessonId = null)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var identityName = User.Identity.Name;

            var currentUser = await _context.User
                .FirstOrDefaultAsync(u =>
                    u.Email == identityName || u.Username == identityName);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy khóa học & bài học
            var course = await _context.KhoaHoc
                .Include(k => k.BaiHoc)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var lessons = course.BaiHoc
                .OrderBy(b => b.ThuTuHienThi)
                .ThenBy(b => b.Id)
                .ToList();

            var totalLessons = lessons.Count;

            // Lấy tiến độ
            var progresses = await _context.TienDoHocTap
                .Where(t => t.UserId == currentUser.UserId && t.KhoaHocId == id)
                .ToListAsync();

            var completedIds = progresses
                .Where(t => t.DaHoanThanh)
                .Select(t => t.BaiHocId)
                .Distinct()
                .ToList();

            var completedCount = completedIds.Count;

            int progressPercent = 0;
            if (totalLessons > 0)
            {
                progressPercent = (int)Math.Round(completedCount * 100.0 / totalLessons);
            }

            // Điểm danh + thống kê (nếu có)
            var sessions = await _context.BuoiHoc
                .Where(b => b.KhoaHocId == id)
                .OrderBy(b => b.ThuTuBuoi)
                .ToListAsync();

            var attendances = await _context.DiemDanh
                .Where(d => d.UserId == currentUser.UserId && d.KhoaHocId == id)
                .Include(d => d.BuoiHoc)
                .ToListAsync();

            // Tính thống kê đơn giản
            var stats = new CourseLearningStatsViewModel();
            if (sessions.Count > 0)
            {
                var totalSession = sessions.Count;
                var attended = attendances.Count(d => d.TrangThai == "present" || d.TrangThai == "late");
                stats.AttendanceRate = (int)Math.Round(attended * 100.0 / totalSession);
            }
            stats.AvgScore = 0;

            var vm = new CourseLearningViewModel
            {
                KhoaHoc = course,
                Lessons = lessons,
                TotalLessons = totalLessons,
                CompletedLessons = completedCount,
                CompletedLessonIds = completedIds,
                ProgressPercent = progressPercent,
                Sessions = sessions,
                Attendances = attendances,
                Stats = stats
            };

            // truyền bài đang học để View biết bài nào cho phép hiện nút hoàn thành
            ViewBag.ActiveLessonId = activeLessonId;

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteLesson(int courseId, int lessonId)
        {
            // 1. Check đăng nhập
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var identityName = User.Identity.Name;

            var currentUser = await _context.User
                .FirstOrDefaultAsync(u =>
                    u.Email == identityName || u.Username == identityName);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Lấy bài học
            var lesson = await _context.BaiHoc
                .FirstOrDefaultAsync(b => b.Id == lessonId && b.KhoaHocId == courseId);

            if (lesson == null)
            {
                return NotFound();
            }

            // 3. BẮT BUỘC HOÀN THÀNH BÀI TRƯỚC ĐÓ
            // Tìm bài học ngay trước nó trong khóa (theo ThuTuHienThi)
            var previousLesson = await _context.BaiHoc
                .Where(b => b.KhoaHocId == courseId
                            && b.ThuTuHienThi < lesson.ThuTuHienThi)
                .OrderByDescending(b => b.ThuTuHienThi)
                .FirstOrDefaultAsync();

            if (previousLesson != null)
            {
                var prevProgress = await _context.TienDoHocTap
                    .FirstOrDefaultAsync(t =>
                        t.UserId == currentUser.UserId &&
                        t.KhoaHocId == courseId &&
                        t.BaiHocId == previousLesson.Id &&
                        t.DaHoanThanh);

                if (prevProgress == null)
                {
                    TempData["Error"] = "Bạn cần hoàn thành bài học trước đó trước khi đánh dấu bài này.";
                    return RedirectToAction("Details", new { id = courseId });
                }
            }
            // Nếu không có previousLesson (tức là bài đầu tiên) thì cho phép tick bình thường.

            // 4. Tìm hoặc tạo record tiến độ
            var progress = await _context.TienDoHocTap
                .FirstOrDefaultAsync(t =>
                    t.UserId == currentUser.UserId &&
                    t.KhoaHocId == courseId &&
                    t.BaiHocId == lessonId);

            if (progress == null)
            {
                progress = new TienDoHocTap
                {
                    UserId = currentUser.UserId,
                    KhoaHocId = courseId,
                    BaiHocId = lessonId,
                    ThoiGianBatDau = DateTime.Now
                };
                _context.TienDoHocTap.Add(progress);
            }

            // 5. Cập nhật trạng thái hoàn thành
            progress.DaHoanThanh = true;
            progress.TrangThaiHoc = "DaHoanThanh";
            progress.ThoiGianHoanThanh = DateTime.Now;
            progress.ThoiGianCapNhat = DateTime.Now;
            progress.TyLeHoanThanh = 100;

            // ThoiLuong là int? nên phải xử lý nullable
            if (lesson.ThoiLuong.HasValue && lesson.ThoiLuong.Value > 0)
            {
                progress.ThoiGianHoc = lesson.ThoiLuong.Value;
            }
            else
            {
                progress.ThoiGianHoc = 45; // mặc định nếu muốn
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = courseId });
        }



    }
}
