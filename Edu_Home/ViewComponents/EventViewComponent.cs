using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Edu_Home.ViewComponents
{
    
        public class EventViewComponent : ViewComponent
        {
            private readonly EduDbContext _eduDbContext;

            public EventViewComponent(EduDbContext eduDbContext)
            {
                _eduDbContext = eduDbContext;
            }
            public async Task<IViewComponentResult> InvokeAsync()
            {
                var newEvent = await _eduDbContext.Events.Where(e => !e.IsDeleted).ToListAsync();
                return View(newEvent);
            }


        }
    
}
