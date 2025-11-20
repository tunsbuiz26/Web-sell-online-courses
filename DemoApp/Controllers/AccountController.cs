using System.Security.Claims;
using DemoApp.Attributes;
using DemoApp.Data;
using DemoApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;


        public AccountController(AppDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // Trim và decode URL
            var rv = returnUrl?.Trim();

            // Decode nếu là encoded URL
            if (!string.IsNullOrEmpty(rv))
            {
                rv = Uri.UnescapeDataString(rv).Trim();
            }

            // Nếu ReturnUrl trỏ về chính trang Login → bỏ
            var loginPath = Url.Action("Login", "Account");
            if (string.IsNullOrWhiteSpace(rv) ||
                rv == "/" ||
                rv.Equals(loginPath, StringComparison.OrdinalIgnoreCase) ||
                rv.StartsWith(loginPath + "?", StringComparison.OrdinalIgnoreCase))
            {
                rv = null;
            }

            ViewBag.ReturnUrl = rv;
            ViewData["ReturnUrl"] = rv;

            return View(new LoginViewModel());
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            // Trim và decode URL
            returnUrl = returnUrl?.Trim();
            if (!string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Uri.UnescapeDataString(returnUrl).Trim();
            }

            // Nếu vẫn null, lấy từ form
            returnUrl ??= Request.Form["returnUrl"].ToString()?.Trim();

            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var username = (model.Username ?? string.Empty).Trim().ToLower();
            var password = (model.Password ?? string.Empty).Trim();

            try
            {
                var user = await _context.User
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u =>
                        u.Username.ToLower() == username &&
                        u.Password == password);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                    return View(model);
                }

                var roleName = user.Role?.RoleName ?? "User";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                    new Claim("FullName", user.FullName ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, roleName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProps = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProps);

                _logger.LogInformation("User {User} login thành công với role {Role}", user.Username, roleName);

                // ===== XỬ LÝ RETURN URL AN TOÀN (ĐÃ SỬA) =====
                string safeReturnUrl = null;

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    // Loại bỏ các ký tự đặc biệt
                    returnUrl = returnUrl.TrimStart('/').TrimEnd('/');

                    if (roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        safeReturnUrl = "/" + returnUrl;
                    }
                    else
                    {
                        // User thường: KHÔNG cho quay về vùng /admin
                        if (!returnUrl.StartsWith("admin", StringComparison.OrdinalIgnoreCase))
                        {
                            safeReturnUrl = "/" + returnUrl;
                        }
                    }
                }

                if (safeReturnUrl != null)
                {
                    return LocalRedirect(safeReturnUrl);
                }

                // Nếu là Admin → vào Dashboard
                if (roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                // Còn lại → về Home
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống khi login");
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra, vui lòng thử lại sau.");
                return View(model);
            }
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // TODO: Implement registration logic
                // 1. Kiểm tra số điện thoại đã tồn tại chưa
                // 2. Hash password
                // 3. Lưu user vào database
                // 4. Gửi email xác nhận (nếu có)
                // 5. Tự động đăng nhập hoặc redirect đến trang login

                // Ví dụ:
                // var userExists = await _userService.CheckUserExists(model.PhoneNumber);
                // if (userExists)
                // {
                //     ModelState.AddModelError("PhoneNumber", "Số điện thoại đã được đăng ký");
                //     return View(model);
                // }

                // var hashedPassword = HashPassword(model.Password);
                // var user = new User
                // {
                //     FullName = model.FullName,
                //     PhoneNumber = model.PhoneNumber,
                //     Email = model.Email,
                //     DateOfBirth = model.DateOfBirth,
                //     PasswordHash = hashedPassword,
                //     CreatedAt = DateTime.UtcNow
                // };

                // await _userService.CreateUser(user);

                // Thành công - redirect về trang đăng nhập
                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra trong quá trình đăng ký. Vui lòng thử lại.");
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // KHÔNG xoá bừa các cookie khác, chỉ sign-out là đủ
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        [AccessDenied]
        public IActionResult AccessDenied()
        {
            return View();
        }
        // Helper method để kiểm tra URL có phải local không (bảo mật)
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Product");
            }
        }
    }
}