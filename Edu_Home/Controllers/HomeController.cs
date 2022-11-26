using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Edu_Home.Controllers
{
    public class HomeController : Controller
    {
        

        public HomeController(ILogger<HomeController> logger)
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }

        
    }
}