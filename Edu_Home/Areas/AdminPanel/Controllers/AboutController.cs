using Edu_Home.Areas.AdminPanel.Data;
using Edu_Home.Areas.AdminPanel.Models;
using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Edu_Home.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class AboutController : BaseController
    {
        private readonly EduDbContext _eduDbContext;
        private readonly IWebHostEnvironment _environment;

        public AboutController(EduDbContext eduDbContext, IWebHostEnvironment environment)
        {
            _eduDbContext = eduDbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var about = await _eduDbContext.Abouts.ToListAsync();
            return View(about);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                return View();
            }

            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Şəkilin ölçüsü 5MB artıq olmamalıdır");
                return View();
            }
            var unicalName= await model.Image.GenerateFile(Constants.AboutPath);

            var about = new About
            {
                ImageUrl = unicalName,
                Title = model.Title,
                Description = model.Description,
                ButtonText = model.ButtonText,
            };

            await _eduDbContext.Abouts.AddAsync(about);
            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var abouts = await _eduDbContext.Abouts.FindAsync(id);

            if(abouts == null) return NotFound();
            var aboutViewModel = new AboutUpdateModel
            {
                Id = abouts.Id,
                Title = abouts.Title,
                Description = abouts.Description,
                ButtonText = abouts.ButtonText,
                ImageUrl = abouts.ImageUrl,
            };

            return View(aboutViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, AboutUpdateModel model)
        {
            if(id== null) return NotFound();
            var abouts = await _eduDbContext.Abouts.FindAsync(id);
            if(abouts == null) return NotFound();
            if (abouts.Id != id) return BadRequest();

            if(model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new AboutUpdateModel
                    {
                        ImageUrl = model.ImageUrl,
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                    return View(new AboutUpdateModel
                    {
                        ImageUrl = abouts.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Şəkil ölçüsü 5MB artıq olmamalıdır");

                    return View(model);
                }

                var unicalPath = await model.Image.GenerateFile(Constants.AboutPath);
                abouts.ImageUrl = unicalPath;
            }

            abouts.Title = model.Title;
            abouts.Description = model.Description;
            abouts.ButtonText = model.ButtonText;

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var about = await _eduDbContext.Abouts.FindAsync(id);

            if(about == null) return NotFound();
            if(about.ImageUrl==null) return NotFound();
            if(about.Id !=id) return BadRequest();
            var aboutPath = Path.Combine(Constants.RootPath, "img", "about", about.ImageUrl);

            if (System.IO.File.Exists(aboutPath))
                System.IO.File.Delete(aboutPath);

            _eduDbContext.Abouts.Remove(about);

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
