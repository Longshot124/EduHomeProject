using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.Controllers
{
    public class AboutController : Controller
    {
        private readonly EduDbContext _eduDbContext;

        public AboutController(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public IActionResult Index()
        {
            var about = _eduDbContext.Abouts.FirstOrDefault();
            return View(about);
        }
       
    }
}
