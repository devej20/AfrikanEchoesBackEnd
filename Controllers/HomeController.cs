using AfrikanEchoes.Entities;
using AfrikanEchoes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AfrikanEchoes.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AfrikanEchoesDbContext _context;

        public HomeController(AfrikanEchoesDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var afrikanEchoesDBContext = _context.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Narrator).Include(b => b.Publisher);
            return View(await afrikanEchoesDBContext.ToListAsync());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public async Task<IActionResult> LandingPage()
        {
            var afrikanEchoesDBContextV5 = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Narrator)
                .Include(b => b.Publisher)
                .Take(10);
            return View(await afrikanEchoesDBContextV5.ToListAsync());
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }
    }
}
