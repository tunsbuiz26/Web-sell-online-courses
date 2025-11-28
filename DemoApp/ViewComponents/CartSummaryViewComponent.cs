using DemoApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DemoApp.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public CartSummaryViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int count = 0;

            // LẤY USER ID HIỆN TẠI TỪ CLAIMS (Identity / Cookie auth)
            // Claim mặc định lưu Id user thường là NameIdentifier
            var user = HttpContext.User;

            if (user?.Identity != null && user.Identity.IsAuthenticated)
            {
                // Lấy user id từ claim
                var idStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
                // Nếu anh có claim "UserId" riêng thì có thể dùng:
                // var idStr = user.FindFirst("UserId")?.Value;

                if (int.TryParse(idStr, out var userId))
                {
                    count = await _context.CartItems
                        .Where(ci => ci.Cart.UserId == userId)
                        .CountAsync();
                }
            }

            return View(count);
        }
    }
}
