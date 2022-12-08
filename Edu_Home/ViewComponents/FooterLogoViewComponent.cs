using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.ViewComponents
{
    public class FooterLogoViewComponent : ViewComponent
    {
        private readonly EduDbContext _eduDbContext;

        public FooterLogoViewComponent(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerLogos = await _eduDbContext.FooterLogos.Where(c => !c.IsDeleted).ToListAsync();
            return View(footerLogos);
        }
    }
}
