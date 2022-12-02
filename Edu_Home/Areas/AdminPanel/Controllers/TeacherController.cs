using Edu_Home.Areas.AdminPanel.Data;
using Edu_Home.Areas.AdminPanel.Models;
using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Edu_Home.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly EduDbContext _eduDbContext;
        private readonly IWebHostEnvironment _environment;

        public TeacherController(EduDbContext eduDbContext, IWebHostEnvironment environment)
        {
            _eduDbContext = eduDbContext;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            var teachers = await _eduDbContext.Teachers.ToListAsync();
            return View(teachers);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!model.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                return View(model);
            }

            if (model.Image.Length > 1024 * 1024 * 2042)
            {
                ModelState.AddModelError("Image", "Şəkil ölçüsü 1MB artıq olmamalıdır");
                return View(model);
            }

            var unicalName = $"{Guid.NewGuid}-{model.Image.FileName}";
            var path = Path.Combine(_environment.WebRootPath, "img/teacher", unicalName);
            var fs = new FileStream(path, FileMode.Create);

            await model.Image.CopyToAsync(fs);

            await _eduDbContext.Teachers.AddAsync(new Teacher
            {
                ImageUrl = unicalName,
                FullName = model.FullName,
                Position = model.Position,
                About = model.About,
                Degree = model.Degree,
                Experience = model.Experience,
                Hobbies = model.Hobbies,
                Faculty = model.Faculty,
                Mail = model.Mail,
                PhoneNumber = model.PhoneNumber,
                Skype = model.Skype,
                LanguageSkill = model.LanguageSkill,
                DesignSkill = model.DesignSkill,
                TeamLiderSkill = model.TeamLiderSkill,
                CommunicationSkill = model.CommunicationSkill,
                DevelopmentSkill = model.DevelopmentSkill,
                InnovationSkill = model.InnovationSkill,

            });


            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            //if (id == null) return BadRequest();
            if (id == null) return NotFound();

            var teachers = await _eduDbContext.Teachers.FindAsync(id);

            //var teachers = await _eduDbContext.Teachers
            //    .Where(t => t.Id == id).FirstOrDefaultAsync();
            if (teachers == null) return NotFound();

            var teacherModel = new TeacherUpdateModel
            {
                Id = teachers.Id,
                FullName = teachers.FullName,
                Position = teachers.Position,
                About = teachers.About,
                Degree = teachers.Degree,
                Experience = teachers.Experience,
                Hobbies = teachers.Hobbies,
                Faculty = teachers.Faculty,
                Mail = teachers.Mail,
                ImageUrl = teachers.ImageUrl,
                PhoneNumber = teachers.PhoneNumber,
                Skype = teachers.Skype,
                LanguageSkill = teachers.LanguageSkill,
                DesignSkill = teachers.DesignSkill,
                TeamLiderSkill = teachers.TeamLiderSkill,
                CommunicationSkill = teachers.CommunicationSkill,
                DevelopmentSkill = teachers.DevelopmentSkill,
                InnovationSkill = teachers.InnovationSkill,
            };
            return View(teacherModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, TeacherUpdateModel model)
        {
            if (id == null) return NotFound();

            var teachers = await _eduDbContext.Teachers.FindAsync(id);
            //var teachers = await _eduDbContext.Teachers
            //    .Where(t => t.Id == id).FirstOrDefaultAsync();

            if (teachers == null) return NotFound();

            if (teachers.Id != id) return BadRequest();

            

            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new TeacherUpdateModel
                    {
                        ImageUrl = teachers.ImageUrl
                    });
                }

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçməlisiniz");

                    return View(new TeacherUpdateModel
                    {
                        ImageUrl = teachers.ImageUrl
                    });
                }

                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Şəkil ölçüsü 5MB artıq olmamalıdır");

                    return View(model);

                }
                var unicalName = await model.Image.GenerateFile(Constants.TeacherPath);

                teachers.ImageUrl = unicalName;
            }
  
            teachers.FullName = model.FullName;
            teachers.Position = model.Position;
            teachers.Degree = model.Degree;
            teachers.Experience = model.Experience;
            teachers.Mail = model.Mail;
            teachers.About = model.About;
            teachers.Hobbies=model.Hobbies;
            teachers.Faculty=model.Faculty;
            teachers.PhoneNumber=model.PhoneNumber;
            teachers.Skype=model.Skype;
            teachers.LanguageSkill=model.LanguageSkill;
            teachers.DesignSkill=model.DesignSkill;
            teachers.TeamLiderSkill=model.TeamLiderSkill;
            teachers.CommunicationSkill = model.CommunicationSkill;
            teachers.DevelopmentSkill = model.DevelopmentSkill;
            teachers.InnovationSkill = model.InnovationSkill;

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teacherImage = await _eduDbContext.Teachers.FindAsync(id);

            if (teacherImage == null) return NotFound();

            if (teacherImage.Id != id) return BadRequest();

            var unicalPath = Path.Combine(Constants.TeacherPath);

            if (System.IO.File.Exists(unicalPath))
                System.IO.File.Delete(unicalPath);

            _eduDbContext.Teachers.Remove(teacherImage);

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


        }
    }
    
}
