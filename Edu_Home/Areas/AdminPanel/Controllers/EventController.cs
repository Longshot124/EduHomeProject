using Edu_Home.Areas.AdminPanel.Data;
using Edu_Home.Areas.AdminPanel.Models;
using Edu_Home.DAL;
using Edu_Home.DAL.Entities;
using Edu_Home.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.Areas.AdminPanel.Controllers
{
    public class EventController :  BaseController
    {
        private readonly EduDbContext _eduDbContext;
       
        public EventController(EduDbContext eduDbContext)
        {
            _eduDbContext = eduDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _eduDbContext
                .Events.Include(e => e.speakerEvents)
                .ThenInclude(e => e.Speaker)
                .Where(s => !s.IsDeleted)
                .OrderByDescending(e => e.Id)
                .ToListAsync();
            return View(events);
        }
        public async Task<IActionResult> Create()
        {
            var speaker = await _eduDbContext.Speakers.ToListAsync();

            var eventSpeakersListItem = new List<SelectListItem>();

            speaker.ForEach(s => eventSpeakersListItem.Add(new SelectListItem(s.FullName, s.Id.ToString())));
            var model = new EventCreateModel
            {
                Speakers = eventSpeakersListItem
            };
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.Start) >= 0)
            {
                ModelState.AddModelError("Start", "Tarixi düzgün seçməlisiz(Tarixin başlama vaxtı gələcəkdə olmalıdı=)))");
                return View(model);
            }
            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.End) >= 0)
            {

                ModelState.AddModelError("End", "Bitmə tarixini düzgün seçin");
                return View(model);
            }
            if (DateTime.Compare(model.Start, model.End) >= 0)
            {
                ModelState.AddModelError("", "Tarixi düzgün seçin, Baaşlama tarixi bitmə tarixindən sonra ola bilməz");
                return View(model);
            }
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                return View();
            }
            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Şəkilin ölmüsü 5MB artıq olmamalıdır");
                return View();
            }
            var unicalFile = await model.Image.GenerateFile(Constants.EventPath);

            var newEvent = new Event
            {
                ImageUrl = unicalFile,
                Adress = model.Adress,
                Content = model.Content,
                Title = model.Title,
                Start = model.Start,
                End = model.End,
            };

            List<SpeakerEvent> speakerEvents = new List<SpeakerEvent>();

            foreach (var speakerId in model.SpeakersId)
            {
                if (!await _eduDbContext.Speakers.AllAsync(e => e.Id == speakerId))
                {
                    ModelState.AddModelError("", "Speaker mövcud deyil");
                }
                speakerEvents.Add(new SpeakerEvent
                {
                    SpeakerId = speakerId,
                });
            }

            var speakers = await _eduDbContext.Speakers.Where(e => !e.IsDeleted).ToListAsync();
            var speakerSelectedItem = new List<SelectListItem>();

            speakers.ForEach(e => speakerSelectedItem.Add(new SelectListItem(e.FullName, e.Id.ToString())));

            newEvent.speakerEvents = speakerEvents;
            model.Speakers = speakerSelectedItem;
            await _eduDbContext.Events.AddAsync(newEvent);
            await _eduDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var newEvent = await _eduDbContext.Events
                .Include(e => e.speakerEvents)
                .ThenInclude(e => e.Speaker)
                .Where(e => !e.IsDeleted && e.Id == id)
                .FirstOrDefaultAsync();

            if (newEvent == null) return NotFound();

            var speakers = await _eduDbContext.Speakers.Where(e => !e.IsDeleted).ToListAsync();

            var eventSpeakerListItem = new List<SelectListItem>();
            speakers.ForEach(e => eventSpeakerListItem.Add(new SelectListItem(e.FullName, e.Id.ToString())));
            List<SpeakerEvent> speakerEvents = new List<SpeakerEvent>();

            foreach (SpeakerEvent eSpeaker in newEvent.speakerEvents)
            {
                if (!await _eduDbContext.Speakers.AnyAsync(e => e.Id == eSpeaker.SpeakerId))
                {
                    ModelState.AddModelError("", "Bele speaker yoxdu");
                    return View();
                }
                speakerEvents.Add(new SpeakerEvent
                {
                    SpeakerId = eSpeaker.Id,
                });

            }
            var eventUpdateModel = new EventUpdateModel
            {
                Title = newEvent.Title,
                Content = newEvent.Content,
                Adress = newEvent.Adress,
                ImageUrl = newEvent.ImageUrl,
                Speakers = eventSpeakerListItem,
                SpeakersId = speakerEvents.Select(e => e.SpeakerId).ToList(),
                Start = newEvent.Start,
                End = newEvent.End,
            };

            return View(eventUpdateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, EventUpdateModel model)
        {
            if (id == null) return NotFound();

            var newEvent = await _eduDbContext.Events
                .Include(e => e.speakerEvents)
                .ThenInclude(e => e.Speaker)
                .Where(e => !e.IsDeleted && e.Id == id)
                .FirstOrDefaultAsync();

            if (newEvent is null) return NotFound();

            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.Start) >= 0)
            {
                ModelState.AddModelError("Start", "Tarixi düzgün seçməlisiz(Tarixin başlama vaxtı gələcəkdə olmalıdı=)))");
                return View(model);
            }
            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.End) >= 0)
            {

                ModelState.AddModelError("End", "Bitmə tarixini düzgün seçin");
                return View(model);
            }
            if (DateTime.Compare(model.Start, model.End) >= 0)
            {
                ModelState.AddModelError("", "Tarixi düzgün seçin, Baaşlama tarixi bitmə tarixindən sonra ola bilməz");
                return View(model);
            }
            var speakers = await _eduDbContext.Speakers.Where(e => !e.IsDeleted).ToListAsync();
            var speakerLsit = new List<SelectListItem>();
            speakers.ForEach(e => speakerLsit.Add(new SelectListItem(e.FullName, e.Id.ToString())));
            List<SpeakerEvent> speakerEvents = new List<SpeakerEvent>();
            if (model.SpeakersId.Count > 0)
            {
                foreach(int speakerId in model.SpeakersId)
                {
                    if(!await _eduDbContext.Speakers.AnyAsync(e => e.Id == speakerId))
                    {
                        ModelState.AddModelError("", "Düzgün speaker seçin!!!");
                        return View(model);
                    }
                    speakerEvents.Add(new SpeakerEvent
                    {
                        SpeakerId = speakerId
                    });
                }
                newEvent.speakerEvents = speakerEvents;
            }
            else
            {
                ModelState.AddModelError("", "Minimum 1 Speaker seçilməlidir");
                return View(model);
            }
            if(model.ImageUrl is not null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                    return View(model);
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Şəkilin ölçüsü 5MB artıq ola bilməz");
                    return View(model);
                }

                var unicalFile = await model.Image.GenerateFile(Constants.EventPath);
                newEvent.ImageUrl=unicalFile;
            }
            var viewModel = new EventUpdateModel
            {
                Speakers = speakerLsit,
            };
            if(!ModelState.IsValid)
                return View(viewModel);
            newEvent.Title = model.Title;
            newEvent.Adress=model.Adress;
            newEvent.Content=model.Content;
            newEvent.Start = model.Start;
            newEvent.End = model.End;
            await _eduDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var existedEvent = await _eduDbContext.Events.FindAsync(id);
            if(existedEvent is null) return NotFound();
            if(existedEvent.Id != id) return NotFound();

            var eventImage = Path.Combine(Constants.RootPath, "img", "event", existedEvent.ImageUrl);
            if(System.IO.File.Exists(eventImage))
               System.IO.File.Delete(eventImage);
            
            _eduDbContext.Events.Remove(existedEvent);
            await _eduDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
