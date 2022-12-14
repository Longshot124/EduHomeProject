using Edu_Home.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.DAL
{
    public class EduDbContext : IdentityDbContext<User>
    {
        public EduDbContext(DbContextOptions<EduDbContext> options) : base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<SpeakerEvent> SpeakerEvents { get; set; }
        public DbSet<FooterLogo> FooterLogos { get; set; }
        public DbSet<FooterContact> FooterContacts { get; set; }
        
        
    }
}
