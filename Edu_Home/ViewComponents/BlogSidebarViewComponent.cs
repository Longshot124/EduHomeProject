using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Edu_Home.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Edu_Home.ViewComponents
{
    public class BlogSidebarViewComponent : ViewComponent
    {
        private readonly EduDbContext _eduDbContext;

        public BlogSidebarViewComponent(EduDbContext dbContext)
        {
            _eduDbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _eduDbContext.Categories.Where(c => !c.IsDeleted).Include(c => c.Courses).ToListAsync();
            var blogs = await _eduDbContext.Blogs.Where(b => !b.IsDeleted).OrderByDescending(b => b.Id).ToListAsync();
            var model = new BlogSidebarViewModel
            {
                Categories = categories,
                Blogs = blogs
            };
            return View(model);

        }
    }
        

}
