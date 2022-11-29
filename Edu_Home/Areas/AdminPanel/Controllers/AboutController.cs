using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class AboutController : BaseController
    {
        private readonly EduDbContext _eduDbContext;
        private readonly IWebHostEnvironment _environment;

        public AboutController(EduDbContext eduDbContext, IWebHostEnvironment environment)
        {
            _eduDbContext = eduDbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var about = await _eduDbContext.About.ToListAsync();
            return View(about);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
    }
}
