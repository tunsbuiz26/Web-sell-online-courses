using System.Security.Claims;
using DemoApp.Attributes;
using DemoApp.Data;
using DemoApp.Models;
using DemoApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

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
            ViewData["ReturnUrl"] = returnUrl;
            ModelState.Clear();  // ⭐ quan trọng

            return View(new LoginViewModel());
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
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

            //if (!ModelState.IsValid)
            //{
            //    foreach (var e in ModelState.Where(x => x.Value.Errors.Count > 0))
            //    {
            //        _logger.LogWarning($"Field {e.Key} ERROR: {string.Join(" | ", e.Value.Errors.Select(err => err.ErrorMessage))}");
            //    }

            //    return View(model);
            //}

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
            // Nếu dữ liệu form không hợp lệ (thiếu field, sai regex, không tick AcceptTerms...)
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // ===== 1. Chuẩn hoá dữ liệu đầu vào =====
                var fullName = (model.FullName ?? string.Empty).Trim();
                var address = (model.Address ?? string.Empty).Trim();
                var phone = (model.PhoneNumber ?? string.Empty).Trim();
                var email = string.IsNullOrWhiteSpace(model.Email)
                                   ? null
                                   : model.Email.Trim();

                // ===== 2. Kiểm tra trùng SĐT =====
                var phoneExists = await _context.User
                    .AnyAsync(u => u.NumberPhone == phone);

                if (phoneExists)
                {
                    ModelState.AddModelError("PhoneNumber", "Số điện thoại đã được đăng ký.");
                    return View(model);
                }

                // ===== 3. Kiểm tra trùng Email (nếu có nhập) =====
                if (!string.IsNullOrEmpty(email))
                {
                    var emailExists = await _context.User
                        .AnyAsync(u => u.Email == email);

                    if (emailExists)
                    {
                        ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                        return View(model);
                    }
                }

                // ===== 4. Tạo username (ở đây dùng luôn số điện thoại) =====
                var username = phone; // Nếu sau này anh muốn tách field Username riêng thì sửa chỗ này

                // Có thể check trùng username luôn cho chắc
                var usernameExists = await _context.User
                    .AnyAsync(u => u.Username == username);

                if (usernameExists)
                {
                    ModelState.AddModelError("", "Tài khoản với số điện thoại này đã tồn tại.");
                    return View(model);
                }

                // ===== 5. Gán RoleId cho User thường =====
                // Giả sử: 1 = Admin, 2 = User (anh chỉnh lại nếu DB khác)
                const int USER_ROLE_ID = 2;

                // Nếu hiện tại login của anh đang SO SÁNH MẬT KHẨU THÔ
                // => tạm thời lưu Password = model.Password.
                // Nếu muốn dùng hash (BCrypt) thì phải sửa luôn cả Login.
                var passwordToStore = model.Password; // hoặc BCrypt.Net.BCrypt.HashPassword(model.Password);

                // ===== 6. Tạo entity User mới =====
                var user = new User
                {
                    FullName = fullName,
                    Address = address,
                    NumberPhone = phone,
                    Email = email,
                    Username = username,
                    Password = passwordToStore,
                    RoleId = USER_ROLE_ID
                };

                // ===== 7. Lưu DB =====
                _context.User.Add(user);
                await _context.SaveChangesAsync();

                // ===== 8. Thông báo & chuyển về trang Login =====
                TempData["RegisterSuccess"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đăng ký tài khoản");

                ModelState.AddModelError(string.Empty,
                    "Có lỗi xảy ra trong quá trình đăng ký. Vui lòng thử lại.");
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