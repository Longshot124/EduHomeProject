using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.Controllers
{
    public class BlogController : Controller
    {
        private readonly EduDbContext _eduDbContext;

        public BlogController(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blog = await _eduDbContext.Blogs.Where(e => !e.IsDeleted).ToListAsync();
            return View(blog);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var blog = await _eduDbContext.Blogs.FirstOrDefaultAsync(e => e.Id == id);
            if (blog.Id == null) return NotFound();
            return View(blog);
        }
    }
}
