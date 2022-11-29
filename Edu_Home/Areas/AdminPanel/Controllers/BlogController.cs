using Edu_Home.Areas.AdminPanel.Data;
using Edu_Home.Areas.AdminPanel.Models;
using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Edu_Home.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class BlogController : BaseController
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiz");
                return View();
            }

            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Şəkilin ölçüsü 5MB artıq olmamalıdır");
                return View();
            }

            var unicalName = await model.Image.GenerateFile(Constants.BlogPath);

            

            var blog = new Blog
            {
                ImageUrl = unicalName,
                Title = model.Title,
                Description = model.Description,
                Author = model.Author,
                Created = DateTime.Now
            };

            await _eduDbContext.Blogs.AddAsync(blog);
            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //public async Task<IActionResult> Create(BlogCreateModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    if (!model.Image.ContentType.Contains("image"))
        //    {
        //        ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
        //        return View(model);
        //    }

        //    if (model.Image.Length > 1024 * 1024 * 2042)
        //    {
        //        ModelState.AddModelError("Image", "Şəkil ölçüsü 1MB artıq olmamalıdır");
        //        return View(model);
        //    }

        //    var unicalName = $"{Guid.NewGuid}-{model.Image.FileName}";
        //    var path = Path.Combine(_environment.WebRootPath, "img/blog", unicalName);
        //    var fs = new FileStream(path, FileMode.Create);

        //    await model.Image.CopyToAsync(fs);

        //    await _eduDbContext.Blogs.AddAsync(new Blog
        //    {

        //        ImageUrl = unicalName,
        //        Title = model.Title,
        //        Description = model.Description,
        //        Author = model.Author,
        //        Created = DateTime.Now
        //    });


        //    await _eduDbContext.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var blogs = await _eduDbContext.Blogs.FindAsync(id);

            if (blogs == null) return NotFound();
            var blogViewModel = new BlogUpdateModel
            {
                ImageUrl=blogs.ImageUrl,
                Id = blogs.Id,
                Title = blogs.Title,
                Description = blogs.Description,
                Author = blogs.Author,
                Created = DateTime.Now,
                

            };

            return View(blogViewModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, BlogUpdateModel model)
        {
            if (id == null) return NotFound();
            var blogs = await _eduDbContext.Blogs.FindAsync(id);
            if (blogs == null) return NotFound();
            if (blogs.Id != id) return BadRequest();

            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new BlogUpdateModel
                    {
                        ImageUrl = blogs.ImageUrl
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                    return View(new BlogUpdateModel
                    {
                        ImageUrl = blogs.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Şəkilin ölçüsü 5MB artıq olmamalıdır");
                    return View(model);
                }
                var unicalPath = await model.Image.GenerateFile(Constants.BlogPath);
                blogs.ImageUrl = unicalPath;
            }

            blogs.Author = model.Author;
            blogs.Description = model.Description;
            blogs.Title = model.Title;
            blogs.Created = model.Created;

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _eduDbContext.Blogs.FindAsync(id);

            if (blog == null) return NotFound();

            if (blog.ImageUrl == null) return NotFound();

            if (blog.Id != id) return BadRequest();

            var blogPath = Path.Combine(Constants.BlogPath, "img", "blog", blog.ImageUrl);

            if (System.IO.File.Exists(blogPath))
                System.IO.File.Delete(blogPath);

            _eduDbContext.Blogs.Remove(blog);

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
