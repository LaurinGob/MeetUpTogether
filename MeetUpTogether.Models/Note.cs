namespace MeetUpTogether.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}
