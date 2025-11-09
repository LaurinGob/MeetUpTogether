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
            set { _currentMeeting = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsEditable)); UpdateCommands(); }
        }

        private string _newNoteText;
        public string NewNoteText
        {
            get => _newNoteText;
            set { _newNoteText = value; OnPropertyChanged(); UpdateCommands(); }
        }

        public bool IsEditable => CurrentMeeting != null;

        public ICommand AddNoteCommand { get; }
        public ICommand SaveMeetingCommand { get; }

        public Action<Meeting> OnSave { get; set; }

        public RightPanelViewModel(MeetingManagerService meetingService)
        {
            _meetingService = meetingService ?? throw new ArgumentNullException(nameof(meetingService));

            AddNoteCommand = new RelayCommand(AddNote, () => IsEditable && !string.IsNullOrWhiteSpace(NewNoteText));
            SaveMeetingCommand = new RelayCommand(SaveMeeting, () => IsEditable && !string.IsNullOrWhiteSpace(CurrentMeeting?.Title));
        }

        private void AddNote()
        {
            if (CurrentMeeting != null && !string.IsNullOrWhiteSpace(NewNoteText))
            {
                var note = new Note { Content = NewNoteText, Meeting = CurrentMeeting, MeetingId = CurrentMeeting.Id };
                CurrentMeeting.Notes.Add(note);
                NewNoteText = string.Empty;
            }
        }

        private void SaveMeeting()
        {
            if (CurrentMeeting == null) return;

            bool isNew = !_meetingService.GetAllMeetings().Contains(CurrentMeeting);

            if (isNew)
                _meetingService.AddMeeting(CurrentMeeting);
            else
                _meetingService.UpdateMeeting(CurrentMeeting);

            OnSave?.Invoke(CurrentMeeting);

            CurrentMeeting = null;
            NewNoteText = string.Empty;
        }

        private void UpdateCommands()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
