using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly EduDbContext _eduDbContext; 

        public BlogViewComponent(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blog = await _eduDbContext.Blogs.Where(e => !e.IsDeleted).ToListAsync();
            return View(blog);
        }
    }
}
