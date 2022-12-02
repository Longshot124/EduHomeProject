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
    public class SpeakerController : BaseController
    {
        private readonly EduDbContext _eduDbContext;
        private readonly IWebHostEnvironment _environment;

        public SpeakerController(EduDbContext eduDbContext, IWebHostEnvironment environment)
        {
            _eduDbContext = eduDbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var speakers = await _eduDbContext.Speakers.ToListAsync();
            return View(speakers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpeakerCreateModel model)
        {
            if(!ModelState.IsValid)
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

            var unicalName = await model.Image.GenerateFile(Constants.SpeakerPath);

            var speaker = new Speaker
            {
                ImageUrl = unicalName,
                FullName = model.FullName,
                Profession = model.Profession,
                Company = model.Company,

            };
            await _eduDbContext.Speakers.AddAsync(speaker);
            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var speakers = await _eduDbContext.Speakers.FindAsync(id);

            if (speakers == null) return NotFound();
            var speakerViewModel = new SpeakerUpdateModel
            {
                ImageUrl = speakers.ImageUrl,
                Id = speakers.Id,
                FullName = speakers.FullName,
                Profession=speakers.Profession,
                Company = speakers.Company,
            };

            return View(speakerViewModel);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, SpeakerUpdateModel model)
        {
            if (id == null) return NotFound();
            var speakers = await _eduDbContext.Speakers.FindAsync(id);
            if (speakers == null) return NotFound();
            if (speakers.Id != id) return BadRequest();

            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new SpeakerUpdateModel
                    {
                        ImageUrl = speakers.ImageUrl
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                    return View(new SpeakerUpdateModel
                    {
                        ImageUrl = speakers.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Şəkilin ölçüsü 5MB artıq olmamalıdır");
                    return View(model);
                }
                var unicalPath = await model.Image.GenerateFile(Constants.SpeakerPath);
                speakers.ImageUrl = unicalPath;
            }

            speakers.FullName = model.FullName;
            speakers.Profession = model.Profession;
            speakers.Company = model.Company;
            

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var speaker = await _eduDbContext.Speakers.FindAsync(id);

            if (speaker == null) return NotFound();

            if (speaker.ImageUrl == null) return NotFound();

            if (speaker.Id != id) return BadRequest();

            var speakerPath = Path.Combine(Constants.SpeakerPath, "img", "blog", speaker.ImageUrl);

            if (System.IO.File.Exists(speakerPath))
                System.IO.File.Delete(speakerPath);

            _eduDbContext.Speakers.Remove(speaker);

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
