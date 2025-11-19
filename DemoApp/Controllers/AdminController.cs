using DemoApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            context = _context;
        }
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
