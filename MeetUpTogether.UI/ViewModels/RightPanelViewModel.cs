using MeetUpTogether.Models;
using MeetUpTogether.BLL.Services;
using System;
using System.Windows.Input;

namespace MeetUpTogether.UI.ViewModels
{
    public class RightPanelViewModel : BaseViewModel
    {
        private readonly MeetingManagerService _meetingService;

        private Meeting _currentMeeting;
        public Meeting CurrentMeeting
        {
            get => _currentMeeting;
            set
            {
                _currentMeeting = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEditable));
                UpdateCommands();
            }
        }

        private string _newNoteText;
        public string NewNoteText
        {
            get => _newNoteText;
            set { _newNoteText = value; OnPropertyChanged(); UpdateCommands(); }
        }

        private bool _isEditable;
        public bool IsEditable
        {
            get => _isEditable;
            set { _isEditable = value; OnPropertyChanged(); UpdateCommands(); }
        }

        public ICommand AddNoteCommand { get; }
        public ICommand SaveMeetingCommand { get; }

        public Action<Meeting> OnSave { get; set; }

        public RightPanelViewModel(MeetingManagerService meetingService)
        {
            _meetingService = meetingService ?? throw new ArgumentNullException(nameof(meetingService));

            AddNoteCommand = new RelayCommand(AddNote, CanAddNote);
            SaveMeetingCommand = new RelayCommand(SaveMeeting, CanSaveMeeting);

            IsEditable = false;
        }

        private bool CanAddNote()
        {
            return IsEditable && CurrentMeeting != null && !string.IsNullOrWhiteSpace(NewNoteText);
        }

        private void AddNote()
        {
            if (CurrentMeeting == null) return;

            var newNote = new Note { Content = NewNoteText };
            CurrentMeeting.Notes.Add(newNote);
            NewNoteText = string.Empty;

            // Update meeting in business layer
            _meetingService.UpdateMeeting(CurrentMeeting);
        }

        private bool CanSaveMeeting()
        {
            return IsEditable && CurrentMeeting != null && !string.IsNullOrWhiteSpace(CurrentMeeting.Title);
        }

        private void SaveMeeting()
        {
            if (CurrentMeeting == null) return;

            if (!_meetingService.GetAll().Contains(CurrentMeeting))
            {
                // New meeting → add via service
                _meetingService.AddMeeting(CurrentMeeting);
            }
            else
            {
                // Existing meeting → update via service
                _meetingService.UpdateMeeting(CurrentMeeting);
            }

            OnSave?.Invoke(CurrentMeeting);

            // Disable editing and clear current meeting after saving
            CurrentMeeting = null;
            IsEditable = false;
            NewNoteText = string.Empty;
        }

        private void UpdateCommands()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
