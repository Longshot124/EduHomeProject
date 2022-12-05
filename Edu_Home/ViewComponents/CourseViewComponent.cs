using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Edu_Home.ViewComponents
{
    public class CourseViewComponent : ViewComponent
    {
        private readonly EduDbContext _eduDbContext;

        public CourseViewComponent(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var courses = await _eduDbContext.Courses.Where(c => !c.IsDeleted).ToListAsync();
            return View(courses);
        }
    }
}
