using Edu_Home.DAL.Entities;
using Edu_Home.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Edu_Home.DAL
{
    public class DataInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EduDbContext _eduDbContext;
        private readonly AdminUser _adminUser;
        public DataInitializer(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _eduDbContext = serviceProvider.GetRequiredService<EduDbContext>();
            _adminUser = serviceProvider.GetService<IOptions<AdminUser>>().Value;          
        }

        public async Task SeedData()
        {
            await _eduDbContext.Database.MigrateAsync();

            var roles = new List<string> { Constants.AdminRole};

            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                    continue;


                var result = await _roleManager.CreateAsync(new IdentityRole { Name = role });

                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
                
            }
            var userExist = await _userManager.FindByNameAsync(_adminUser.UserName);

            if (userExist != null)
                return;

            var userResult = await _userManager.CreateAsync(new User
            {
                UserName = _adminUser.UserName,
                Email = _adminUser.Email,
            }, _adminUser.Password);

            if (!userResult.Succeeded)
            {
                foreach (var error in userResult.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }
            else
            {
                var existUser = await _userManager.FindByNameAsync(_adminUser.UserName);

                await _userManager.AddToRoleAsync(existUser, Constants.AdminRole);
            }
        }
    }
}
