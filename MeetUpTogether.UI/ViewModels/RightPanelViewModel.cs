using MeetUpTogether.Models;
using MeetUpTogether.BLL.Services;
using System;
using System.Windows.Input;
using log4net;

namespace MeetUpTogether.UI.ViewModels
{
    public class RightPanelViewModel : BaseViewModel
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RightPanelViewModel));

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

                _logger.Info($"CurrentMeeting changed: {CurrentMeeting?.Title ?? "None"}");
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
                UpdateCommands();

                _logger.Debug($"NewNoteText updated: {NewNoteText}");
            }
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

            _logger.Info("RightPanelViewModel initialized");
        }

        private void AddNote()
        {
            if (CurrentMeeting == null || string.IsNullOrWhiteSpace(NewNoteText)) return;

            try
            {
                var note = new Note
                {
                    Content = NewNoteText,
                    Meeting = CurrentMeeting,
                    MeetingId = CurrentMeeting.Id
                };
                CurrentMeeting.Notes.Add(note);
                _logger.Info($"Added note to meeting '{CurrentMeeting.Title}': {note.Content}");
                NewNoteText = string.Empty;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to add note", ex);
            }
        }

        private void SaveMeeting()
        {
            if (CurrentMeeting == null) return;

            try
            {
                bool isNew = !_meetingService.GetAllMeetings().Contains(CurrentMeeting);

                if (isNew)
                {
                    _meetingService.AddMeeting(CurrentMeeting);
                    _logger.Info($"New meeting added: {CurrentMeeting.Title}");
                }
                else
                {
                    _meetingService.UpdateMeeting(CurrentMeeting);
                    _logger.Info($"Meeting updated: {CurrentMeeting.Title}");
                }

                OnSave?.Invoke(CurrentMeeting);

                // Clear after saving
                _logger.Debug($"Clearing CurrentMeeting and NewNoteText after save");
                CurrentMeeting = null;
                NewNoteText = string.Empty;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to save meeting", ex);
            }
        }

        private void UpdateCommands()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
