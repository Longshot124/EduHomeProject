using Edu_Home.Areas.AdminPanel.Models;
using Edu_Home.DAL.Entities;
using Edu_Home.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class FooterContactController : BaseController
    {
        private readonly EduDbContext _eduDbContext;
        private readonly IWebHostEnvironment _environment;

        public FooterContactController(EduDbContext eduDbContext, IWebHostEnvironment environment)
        {
            _eduDbContext = eduDbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var footerContacts = await _eduDbContext.FooterContacts.ToListAsync();
            return View(footerContacts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FooterContactCreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var footerContact = new FooterContact
            {
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                SecondAddress = model.SecondAddress,
                SecondPhoneNumber = model.SecondPhoneNumber,
                Email = model.Email,
                Website = model.Website,

            };
            await _eduDbContext.FooterContacts.AddAsync(footerContact);
            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var footerContacts = await _eduDbContext.FooterContacts.FindAsync(id);

            if (footerContacts == null) return NotFound();
            var footerContactViewModel = new FooterContactUpdateModel
            {
                Id = footerContacts.Id,
                Address = footerContacts.Address,
                Email = footerContacts.Email,
                PhoneNumber = footerContacts.PhoneNumber,
                SecondPhoneNumber = footerContacts.SecondPhoneNumber,
                Website = footerContacts.Website,
                SecondAddress = footerContacts.SecondAddress,
            };

            return View(footerContactViewModel);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, FooterContactUpdateModel model)
        {
            if (id == null) return NotFound();
            var footerContacts = await _eduDbContext.FooterContacts.FindAsync(id);
            if (footerContacts == null) return NotFound();
            if (footerContacts.Id != id) return BadRequest();

            footerContacts.Address = model.Address;
            footerContacts.SecondAddress = model.SecondAddress;
            footerContacts.PhoneNumber = model.PhoneNumber;
            footerContacts.SecondAddress = model.SecondAddress;
            footerContacts.Website = model.Website;
            footerContacts.Email = model.Email;


            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var footerContacts = await _eduDbContext.FooterContacts.FindAsync(id);

            if (footerContacts == null) return NotFound();

            if (footerContacts.Id != id) return BadRequest();


            _eduDbContext.FooterContacts.Remove(footerContacts);

            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

    }
}

