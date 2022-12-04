using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.Controllers
{
    public class TeacherController : Controller
    {
        private readonly EduDbContext _eduDbContext;

        public TeacherController(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var teacher = await _eduDbContext.Teachers.Where(e => !e.IsDeleted).ToListAsync();
            return View(teacher);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var teacher= await _eduDbContext.Teachers.FirstOrDefaultAsync(e=>e.Id==id);
            if (teacher.Id == null) return NotFound();
            return View(teacher);
        } 
    }
}
