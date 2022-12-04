using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.ViewComponents
{
    public class TeacherViewComponent : ViewComponent
    {
        private readonly EduDbContext _eduDbContext;

        public TeacherViewComponent(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var teacher = await _eduDbContext.Teachers.Where(e => !e.IsDeleted).ToListAsync();
            return View(teacher);
        }
    }
}
