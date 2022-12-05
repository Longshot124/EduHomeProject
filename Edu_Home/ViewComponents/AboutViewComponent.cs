using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.ViewComponents
{
    public class AboutViewComponent : ViewComponent
    {
        private readonly EduDbContext _eduDbContext;

        public AboutViewComponent(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var about = await _eduDbContext.Abouts.Where(e => !e.IsDeleted).ToListAsync();
            return View(about);
        }
    }
}
