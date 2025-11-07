using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUpTogether.UI.Models
{
    public class Meeting
    {
        public string Title { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Agenda { get; set; }
        public ObservableCollection<Note> Notes { get; set; } = new();
    }
}
