    using System.Diagnostics;
using System.Threading.Tasks;
using DemoApp.Data;
using DemoApp.Models;
    using Microsoft.AspNetCore.Mvc;

    namespace DemoApp.Controllers
    {
        public class HomeController : Controller
        {
        private readonly AppDbContext _context;
            private readonly ILogger<HomeController> _logger;

            public HomeController(ILogger<HomeController> logger, AppDbContext context)
            {
                _logger = logger;
                _context = context;
            }

            public IActionResult Index()
            {
                return View();
            }

            public IActionResult Privacy()
            {
                return View();
            }
            
            public async Task<IActionResult> Khoahoc()
        {
            
            return View();
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
