using Edu_Home.Areas.AdminPanel.Data;
using Edu_Home.Areas.AdminPanel.Models;
using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Edu_Home.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class FooterLogoController : BaseController
    {
        
            private readonly EduDbContext _eduDbContext;
            private readonly IWebHostEnvironment _environment;

            public FooterLogoController(EduDbContext eduDbContext, IWebHostEnvironment environment)
            {
                _eduDbContext = eduDbContext;
                _environment = environment;
            }

            public async Task<IActionResult> Index()
            {
                var footerLogos = await _eduDbContext.FooterLogos.ToListAsync();
                return View(footerLogos);
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(FooterLogoCreateModel model)
            {
                if (!ModelState.IsValid)
                    return View(model);
                if (!model.LogoImage.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçməlisiz");
                    return View();
                }
                if (!model.LogoImage.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Şəkilin ölçüsü 5MB artıq olmamalıdır");
                    return View();
                }

                var unicalName = await model.LogoImage.GenerateFile(Constants.FooterLogoPath);

                var footerLogo = new FooterLogo
                {
                    LogoUrl = unicalName,
                    FacebookLink = model.FacebookLink,
                    TwitterLink = model.TwitterLink,
                    VimeoLink = model.VimeoLink,
                    PisterestLink=model.PisterestLink,
                    Description = model.Description,

                };
                await _eduDbContext.FooterLogos.AddAsync(footerLogo);
                await _eduDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            public async Task<IActionResult> Update(int? id)
            {
                if (id == null) return NotFound();

                var footerLogo = await _eduDbContext.FooterLogos.FindAsync(id);

                if (footerLogo == null) return NotFound();
            var footerLogoViewModel = new FooterLogoUpdateModel
            {
                Id = footerLogo.Id,
                LogoUrl = footerLogo.LogoUrl,
                FacebookLink = footerLogo.FacebookLink,
                TwitterLink = footerLogo.TwitterLink,
                VimeoLink = footerLogo.VimeoLink,
                PisterestLink = footerLogo.PisterestLink,
                Description = footerLogo.Description,
            };

                return View(footerLogoViewModel);


            }
            [HttpPost]
            [ValidateAntiForgeryToken]

            public async Task<IActionResult> Update(int? id, FooterLogoUpdateModel model)
            {
                if (id == null) return NotFound();
                var footerLogo = await _eduDbContext.FooterLogos.FindAsync(id);
                if (footerLogo == null) return NotFound();
                if (footerLogo.Id != id) return BadRequest();

                if (model.LogoImage != null)
                {
                    if (!ModelState.IsValid)
                    {
                        return View(new FooterLogoUpdateModel
                        {
                            LogoUrl = footerLogo.LogoUrl
                        });
                    }
                    if (!model.LogoImage.IsImage())
                    {
                        ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                        return View(new FooterLogoUpdateModel
                        {
                            LogoUrl = footerLogo.LogoUrl
                        });
                    }
                    if (!model.LogoImage.IsAllowedSize(5))
                    {
                        ModelState.AddModelError("Image", "Şəkilin ölçüsü 5MB artıq olmamalıdır");
                        return View(model);
                    }

                if (footerLogo.LogoUrl is null) return NotFound();

                var footerImagePath = Path.Combine(Constants.RootPath, "img", "logo", footerLogo.LogoUrl);

                if (System.IO.File.Exists(footerImagePath))
                    System.IO.File.Delete(footerImagePath);

                var unicalPath = await model.LogoImage.GenerateFile(Constants.FooterLogoPath);
                    footerLogo.LogoUrl = unicalPath;
                }

            footerLogo.FacebookLink = model.FacebookLink;
            footerLogo.TwitterLink = model.TwitterLink;
            footerLogo.PisterestLink = model.PisterestLink;
            footerLogo.VimeoLink = model.VimeoLink;
            footerLogo.Description = model.Description;

                await _eduDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            [HttpPost]
            [ValidateAntiForgeryToken]

            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null) return NotFound();

                var footerLogo = await _eduDbContext.FooterLogos.FindAsync(id);

                if (footerLogo == null) return NotFound();

                if (footerLogo.LogoUrl == null) return NotFound();

                if (footerLogo.Id != id) return BadRequest();

                var footerLogoPath = Path.Combine(Constants.FooterLogoPath, "img", "logo", footerLogo.LogoUrl);

                if (System.IO.File.Exists(footerLogoPath))
                    System.IO.File.Delete(footerLogoPath);

                _eduDbContext.FooterLogos.Remove(footerLogo);

                await _eduDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
        
    }
}
