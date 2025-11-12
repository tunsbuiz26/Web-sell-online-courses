using System.Security.Claims;
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

        public AccountController(AppDbContext context)
        {
            context = _context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) // Xóa parameter returnUrl
        {
            if (ModelState.IsValid)
            {
                if (model.Username == "admin" && model.Password == "admin123")
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe ?
                            DateTimeOffset.UtcNow.AddDays(30) :
                            DateTimeOffset.UtcNow.AddHours(24)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Luôn chuyển đến Product/Index
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }

            return View(model);
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
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
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