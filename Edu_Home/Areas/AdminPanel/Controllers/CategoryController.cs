using Edu_Home.Areas.AdminPanel.Models;
using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly EduDbContext _eduDbContext;
        private readonly IWebHostEnvironment _environment;

        public CategoryController(EduDbContext eduDbContext, IWebHostEnvironment environment)
        {
            _eduDbContext = eduDbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var category = await _eduDbContext.Categories.ToListAsync();
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CategoryCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existCategory = await _eduDbContext.Categories.Where(e => !e.IsDeleted).ToListAsync();

            if (existCategory.Any(e => e.Name.ToLower().Trim().Equals(model.Name.ToLower().Trim())))
            {
                ModelState.AddModelError("Name", "Bu kateqoriya mövcuddur!");
                return View();
            }
            var category = new Category
            {
                Name = model.Name,
            };

            await _eduDbContext.Categories.AddAsync(category);
            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var category = await _eduDbContext.Categories.FindAsync(id);
            if (category.Id != id) return BadRequest();

            var existCategory = new CategoryUpdateModel
            {
                Name = category.Name
            };
            return View(existCategory);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryCreateModel model)
        {
            if (id == null) return NotFound();
            var categories = await _eduDbContext.Categories.FindAsync(id);
            if (categories == null) return NotFound();
            if (categories.Id != id) return BadRequest();

            if (categories == null) return NotFound();
            var ExistName = await _eduDbContext.Categories.AnyAsync(e => e.Name.ToLower().Trim() == model.Name.ToLower().Trim() && e.Id != id);

            if (ExistName)
            {
                ModelState.AddModelError("Name", "Daxil etdiyiniz adda kateqoriya  mövcuddur..!");
                return View(model);
            }
            categories.Name = model.Name;

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _eduDbContext.Categories.FindAsync(id);

            if (category == null) return NotFound();
            if (category.Id != id) return BadRequest();

            _eduDbContext.Categories.Remove(category);

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }   
}
