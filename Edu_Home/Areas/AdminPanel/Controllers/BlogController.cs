using Edu_Home.Areas.AdminPanel.Data;
using Edu_Home.Areas.AdminPanel.Models;
using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Edu_Home.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class BlogController : Controller
    {
        private readonly EduDbContext _eduDbContext;
        private readonly IWebHostEnvironment _environment;

        public BlogController(EduDbContext eduDbContext, IWebHostEnvironment environment)
        {
            _eduDbContext = eduDbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _eduDbContext.Blogs.ToListAsync();
            return View(blogs);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Create(BlogCreateModel model)
        {
            if(!ModelState.IsValid) return View(model);

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                return View();
            }

            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Şəkil ölçüsü 1MB artıq olmamalıdır");
                return View();
            }

            var unicalName = await model.Image.GenerateFile(Constants.BlogPath);
            var blog = new Blog
            {
                ImageUrl = unicalName,
                Title=model.Title,
                Description=model.Description,
                Author=model.Author,
                Created=DateTime.Now,

                
            };

            await _eduDbContext.Blogs.AddAsync(blog);
            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> Update(int? id)
        //{
        //    if
        //}
    }
}
