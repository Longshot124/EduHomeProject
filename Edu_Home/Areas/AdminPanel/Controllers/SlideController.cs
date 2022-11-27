using Edu_Home.Areas.AdminPanel.Data;
using Edu_Home.Areas.AdminPanel.Models;
using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Reflection.Metadata;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class SlideController : BaseController
    {
        private readonly EduDbContext _eduDbContext;
        private readonly IWebHostEnvironment _environment;

        public SlideController(EduDbContext eduDbContext, IWebHostEnvironment environment)
        {
            _eduDbContext = eduDbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _eduDbContext.Sliders.ToListAsync();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SlideImageCreateModel model)
        {
            if (!ModelState.IsValid)
                return View();

            if (!model.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                return View();
            }

            if (model.Image.Length > 1024 * 1024 * 2042)
            {
                ModelState.AddModelError("Image", "Şəkil ölçüsü 1MB artıq olmamalıdır");
                return View();
            }

            var unicalName = $"{Guid.NewGuid}-{model.Image.FileName}";
            var path = Path.Combine(_environment.WebRootPath, "img/slider", unicalName);
            var fs = new FileStream(path, FileMode.Create);

            await model.Image.CopyToAsync(fs);

            await _eduDbContext.Sliders.AddAsync(new Slider
            {
                ImageUrl = unicalName,
                Title = model.Title,
                SubTitle = model.SubTitle,
                ButtonText = model.ButtonText,
                //ButtonUrl = model.ButtonUrl
            });


            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return NotFound();

            var slideImage = await _eduDbContext.Sliders.FindAsync(id);

            return View(new SlideImageUpdateModel
            {
                ImageUrl = slideImage.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SlideImageUpdateModel model)
        {
            if (id == null) return NotFound();

            var slideImage = await _eduDbContext.Sliders.FindAsync(id);

            if (slideImage == null) return NotFound();

            if (slideImage.Id != id) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(new SlideImageUpdateModel
                {
                    ImageUrl = slideImage.Name
                });
            }

            if (!model.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiniz");

                return View(new SlideImageUpdateModel
                {
                    ImageUrl = slideImage.Name
                });
            }

            if (model.Image.Length > 1024 * 1024 * 2042)
            {
                ModelState.AddModelError("Image", "Şəkil ölçüsü 1MB artıq olmamalıdır");

                return View(new SlideImageUpdateModel
                {
                    ImageUrl = slideImage.Name
                });
            }

            var unicalPath = Path.Combine(Constants.SliderPath);

            if (System.IO.File.Exists(unicalPath))
                System.IO.File.Delete(unicalPath);

            var unicalName = await model.Image.GenerateFile(Constants.RootPath);

            slideImage.Name = unicalName;
            slideImage.SubTitle = model.SubTitle;
            slideImage.Title = model.Title;
            slideImage.ButtonText = model.ButtonText;

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
