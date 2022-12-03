using System.ComponentModel.DataAnnotations;

namespace Edu_Home.DAL.Entities
{
    public class Event : Entity
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime End { get; set; }
        public string Adress { get; set; }
        public string Content { get; set; }
        public List<SpeakerEvent> speakerEvents { get; set; }
    }
}
