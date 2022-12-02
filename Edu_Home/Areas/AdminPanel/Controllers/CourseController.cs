using Edu_Home.Areas.AdminPanel.Data;
using Edu_Home.Areas.AdminPanel.Models;
using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Edu_Home.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.IO;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class CourseController : BaseController
    {
        private readonly EduDbContext _eduDbContext;
        private readonly IWebHostEnvironment _environment;

        public CourseController(EduDbContext eduDbContext, IWebHostEnvironment environment)
        {
            _eduDbContext = eduDbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _eduDbContext.Courses.Include(e => e.Category).ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _eduDbContext.Categories.Where(e => !e.IsDeleted).ToListAsync();

            var categoryList = new List<SelectListItem>
            {
                new SelectListItem("Category secin" , "0")
            };

            categories.ForEach(e => categoryList.Add(new SelectListItem(e.Name, e.Id.ToString())));
            var model = new CourseCreateModel
            {
                Categories = categoryList
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(int? id, CourseCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var categories = await _eduDbContext.Categories.Where(e => !e.IsDeleted).Include(e => e.Courses).ToListAsync();

            var categoryList = new List<SelectListItem>
            {
                new SelectListItem("Kategoriya seçilməyib","0")

            };
            categories.ForEach(e => categoryList.Add(new SelectListItem(e.Name, e.Id.ToString())));

            var courseViewModel = new CourseCreateModel()
            {
                Categories = categoryList
            };

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

            if (model.CategoryId == 0)
            {
                ModelState.AddModelError("", "Kategoriya seçilməyib");
                return View();
            }
            var unicalName = await model.Image.GenerateFile(Constants.CoursePath);

            var newCourse = new Course
            {
                ImageUrl = unicalName,
                CategoryId = model.CategoryId,
                Title = model.Title,
                Description = model.Description,
                About = model.About,
                Apply = model.Apply,
                Certification = model.Certification,
                Starts = DateTime.Now,
                Duration = model.Duration,
                ClassDuration = model.ClassDuration,
                Skill = model.Skill,
                Language = model.Language,
                Students = model.Students,
                Assesments = model.Assesments,
                Price = model.Price,

            };

            await _eduDbContext.Courses.AddAsync(newCourse);
            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var category = await _eduDbContext.Categories.Where(e => !e.IsDeleted).ToListAsync();
            if (category == null) return NotFound();
            var course = await _eduDbContext.Courses
                .Where(e => !e.IsDeleted && e.Id == id).Include(e => e.Category).FirstOrDefaultAsync();
            if (course == null) return NotFound();
            if (course.Id != id) return NotFound();

            var selectCategory = new List<SelectListItem>();

            var viewModel = new CourseUpdateModel
            {
                Categories = selectCategory
            };
            if (!ModelState.IsValid) return View(viewModel);

            category.ForEach(e => selectCategory.Add(new SelectListItem(e.Name, e.Id.ToString())));

            var courseUpdateModel = new CourseUpdateModel
            {
                Id = course.Id,
                Title = course.Title,
                ImageUrl = course.ImageUrl,
                Description = course.Description,
                About = course.About,
                Apply = course.Apply,
                Certification = course.Certification,
                Starts = DateTime.Now,
                Duration = course.Duration,
                ClassDuration = course.ClassDuration,
                Skill = course.Skill,
                Language = course.Language,
                Students = course.Students,
                Assesments = course.Assesments,
                Price = course.Price,
                Categories = selectCategory,
                CategoryId = course.CategoryId,

            };

            return View(courseUpdateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CourseUpdateModel model)
        {
            if (id == null) return BadRequest();

            if (!ModelState.IsValid) return View();
            var categories = await _eduDbContext.Categories.Where(e => e.IsDeleted).ToListAsync();
            if (categories == null) return NotFound();
            var course = await _eduDbContext.Courses.Where(e => !e.IsDeleted && e.Id == id).Include(e => e.Category).FirstOrDefaultAsync();
            if (course == null) return NotFound();

            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new CourseUpdateModel
                    {
                        ImageUrl = model.ImageUrl,
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                    return View(new CourseUpdateModel
                    {
                        ImageUrl = course.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Şəkil ölçüsü 5MB artıq olmamalıdır");

                    return View(model);
                }

                var unicalPath = Path.Combine(Constants.CoursePath, course.ImageUrl);

                if (System.IO.File.Exists(unicalPath))
                    System.IO.File.Delete(unicalPath);


                var unicalFile = await model.Image.GenerateFile(Constants.CoursePath);
                course.ImageUrl = unicalFile;


            }
            var selectedCategory = new CourseUpdateModel
            {
                CategoryId = model.CategoryId
            };
            course.Title = model.Title;
            course.Description = model.Description;
            course.About = model.About;
            course.Assesments = model.Assesments;
            course.Duration = model.Duration;
            course.ClassDuration = model.ClassDuration;
            course.Skill = model.Skill;
            course.Language = model.Skill;
            course.Students = model.Students;
            course.Apply = model.Apply;
            course.Certification = model.Certification;
            course.Price = model.Price;
            course.CategoryId = model.CategoryId;

            await _eduDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var course = await _eduDbContext.Courses.FirstOrDefaultAsync(e => e.Id == id);

            if (course == null) return NotFound();

            if (course.Id != id) return BadRequest();

            var unicalPath = Path.Combine(Constants.CoursePath, "img", "course", course.ImageUrl);



            if (System.IO.File.Exists(unicalPath))
                System.IO.File.Delete(unicalPath);

            _eduDbContext.Courses.Remove(course);
            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
