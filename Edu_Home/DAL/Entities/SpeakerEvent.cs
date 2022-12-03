namespace Edu_Home.DAL.Entities
{
    public class SpeakerEvent:Entity
    {
        public int SpeakerId { get; set; }
        public Speaker Speaker { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
