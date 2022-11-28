using Edu_Home.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edu_Home.DAL
{
    public class EduDbContext : DbContext 
    {
        public EduDbContext(DbContextOptions<EduDbContext> options) : base(options) 
        {

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}
