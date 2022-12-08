using Edu_Home.DAL.Entities;
using Edu_Home.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Edu_Home.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existUser = await _userManager.FindByNameAsync(model.UserName);

            if (existUser is null)
            {
                ModelState.AddModelError("", "Oops, Gözlənizməz xəta verdi baş =)");
                return View();
            }

            var signResult = await _signInManager.PasswordSignInAsync(existUser, 
                model.Password,
                model.RememberMe,
                true);

            if (!signResult.Succeeded)
            {
                ModelState.AddModelError("", "Oops, Gözlənizməz xəta verdi baş =)");
                return View();
            }
            return RedirectToAction("Index", "Home");    
        }
        public IActionResult Index()
        {
            return View();
        }
       
        
    }
}
