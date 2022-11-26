using Microsoft.AspNetCore.Mvc;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
