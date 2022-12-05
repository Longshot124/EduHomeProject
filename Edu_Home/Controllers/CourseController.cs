using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Edu_Home.Controllers
{
    public class CourseController : Controller
    {
        private readonly EduDbContext _dbContext;

        public CourseController(EduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _dbContext.Courses.Where(c => !c.IsDeleted).Include(c => c.Category).ToListAsync();
            return View(courses);
        }

        public async Task<IActionResult> Details(int? id)

        {
            if (id is null) return NotFound();
            var course = await _dbContext.Courses.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (course is null) return NotFound();


            return View(course);
        }

        public async Task<IActionResult> Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return NoContent();

            var courses = await _dbContext.Courses
                .Where(course => !course.IsDeleted && course.Title.ToLower().Contains(searchText.ToLower()))
                .ToListAsync();

            var model = new List<Course>();

            courses.ForEach(course => model.Add(new Course
            {
                Id = course.Id,
                Title = course.Title,
                ImageUrl = course.ImageUrl,
            }));

            return PartialView("_CourseSearchPartial", courses);
        }

        public async Task<IActionResult> BlogSidebar(int? id)
        {
            var categories = await _dbContext.Categories.Where(c => c.Id == id).Include(c => c.Courses).ToListAsync();
            return View(categories);
        }
    }
}

