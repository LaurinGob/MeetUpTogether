using MeetUpTogether.UI.Models;
using System.Windows.Input;

namespace MeetUpTogether.UI.ViewModels
{
    public class RightPanelViewModel : BaseViewModel
    {
        private bool _isEditable;
        public bool IsEditable
        {
            get => _isEditable;
            set { _isEditable = value; OnPropertyChanged(); }
        }

        private Meeting _currentMeeting;
        public Meeting CurrentMeeting
        {
            get => _currentMeeting;
            set
            {
                _currentMeeting = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _newNoteText;
        public string NewNoteText
        {
            get => _newNoteText;
            set
            {
                _newNoteText = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AddNoteCommand { get; }
        public ICommand SaveMeetingCommand { get; }

        public Action<Meeting> OnSave { get; set; }

        public RightPanelViewModel()
        {
            AddNoteCommand = new RelayCommand(AddNote, () => !string.IsNullOrWhiteSpace(NewNoteText) && CurrentMeeting != null);
            SaveMeetingCommand = new RelayCommand(SaveMeeting, () => CurrentMeeting != null);
        }

        private void AddNote()
        {
            if (CurrentMeeting != null && !string.IsNullOrWhiteSpace(NewNoteText))
            {
                CurrentMeeting.Notes.Add(new Note { Content = NewNoteText });
                NewNoteText = string.Empty;
            }
        }

        private void SaveMeeting()
        {
            if (CurrentMeeting != null)
            {
                OnSave?.Invoke(CurrentMeeting);
                CurrentMeeting = null;
                NewNoteText = string.Empty;
                IsEditable = false;
            }
        }
    }
}