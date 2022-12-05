using Edu_Home.DAL;
using Edu_Home.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Edu_Home.Controllers
{
    public class HomeController : Controller
    {


        private readonly EduDbContext _eduDbContext;
        public HomeController(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var slider = await _eduDbContext.Sliders.ToListAsync();
            var homeViewModel = new HomeViewModel
            {
                Sliders = slider,
            };
            return View(homeViewModel);
        }


    }
}