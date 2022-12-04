using Edu_Home.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles=Constants.AdminRole)]
    public class BaseController : Controller
    {
       
    }
}
