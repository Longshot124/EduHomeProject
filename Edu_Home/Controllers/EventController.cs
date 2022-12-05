using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Edu_Home.Controllers
{
    public class EventController : Controller
    {
        private readonly EduDbContext _eduDbContext;

        public EventController(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _eduDbContext.Events.Where(e => !e.IsDeleted).ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var newEvent = await _eduDbContext.Events.Where(c => !c.IsDeleted && c.Id == id).Include(e => e.speakerEvents).ThenInclude(e => e.Speaker).FirstOrDefaultAsync();
            if (newEvent is null) return NotFound();


            return View(newEvent);
        }
    }
}
