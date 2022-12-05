using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Edu_Home.ViewComponents
{
    public class EventHomeViewComponent : ViewComponent
    {
        private readonly EduDbContext _eduDbContext;

        public EventHomeViewComponent(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var newEvent = await _eduDbContext.Events.Where(e => !e.IsDeleted).OrderByDescending(e => e.Id).ToListAsync();
            return View(newEvent);
        }
    }
}
