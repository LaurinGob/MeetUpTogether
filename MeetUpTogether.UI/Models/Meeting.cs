using System.Collections.ObjectModel;

namespace MeetUpTogether.UI.Models
{
    public class Meeting
    {
        public string Title { get; set; } = "New Meeting";
        public DateTime From { get; set; } = DateTime.Today;
        public DateTime To { get; set; } = DateTime.Today;
        public string Agenda { get; set; }
        public ObservableCollection<Note> Notes { get; set; } = new();
    }
}
