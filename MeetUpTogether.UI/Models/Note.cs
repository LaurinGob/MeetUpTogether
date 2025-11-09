namespace MeetUpTogether.UI.Models
{
    public class Note
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
