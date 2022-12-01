using Edu_Home.Areas.AdminPanel.Data;
using Edu_Home.Areas.AdminPanel.Models;
using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Edu_Home.Data;
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
            if (!ModelState.IsValid) return View(model);

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

            var unicalName = await model.Image.GenerateFile(Constants.SliderPath);

            var slider = new Slider
            {
                ImageUrl = unicalName,
                Title = model.Title,
                SubTitle = model.SubTitle,
                ButtonText = model.ButtonText,               
            };

            await _eduDbContext.Sliders.AddAsync(slider);
            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var sliders = await _eduDbContext.Sliders.FindAsync(id);

            if(sliders == null) return NotFound();
            var sliderViewModel = new SlideImageUpdateModel
            {
                Id = sliders.Id,
                Title = sliders.Title,
                SubTitle = sliders.SubTitle,
                ButtonText = sliders.ButtonText,
                ImageUrl = sliders.ImageUrl,
            };

            return View(sliderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SlideImageUpdateModel model)
        {
            if (id == null) return NotFound();

            var sliders = await _eduDbContext.Sliders.FindAsync(id);

            if (sliders == null) return NotFound();

            if (sliders.Id != id) return BadRequest();

            

            if(model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new SlideImageUpdateModel
                    {
                        ImageUrl = sliders.ImageUrl
                    });
                }

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçməlisiniz");

                    return View(new SlideImageUpdateModel
                    {
                        ImageUrl = sliders.ImageUrl
                    });
                }

                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Şəkil ölçüsü 5MB artıq olmamalıdır");

                    return View(model);
                }
                var unicalPath = await model.Image.GenerateFile(Constants.SliderPath);
                sliders.ImageUrl = unicalPath;
            }          
         
            sliders.SubTitle = model.SubTitle;
            sliders.Title = model.Title;
            sliders.ButtonText = model.ButtonText;

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var slider = await _eduDbContext.Sliders.FindAsync(id);

            if (slider == null) return NotFound();

            if (slider.ImageUrl == null) return NotFound();

            if (slider.Id != id) return BadRequest();
            var sliderPath = Path.Combine(Constants.RootPath, "img", "slider", slider.ImageUrl);

            if (System.IO.File.Exists(sliderPath))
                System.IO.File.Delete(sliderPath);
    
            _eduDbContext.Sliders.Remove(slider);

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
