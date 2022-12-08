using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.ViewComponents
{
    public class FooterContactViewComponent : ViewComponent
    {
        private readonly EduDbContext _eduDbContext;

        public FooterContactViewComponent(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerAdress = await _eduDbContext.FooterContacts.Where(c => !c.IsDeleted).ToListAsync();
            return View(footerAdress);
        }
    }
}
