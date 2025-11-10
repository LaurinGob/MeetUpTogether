using MeetUpTogether.Models;
using MeetUpTogether.BLL.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using log4net;

namespace MeetUpTogether.UI.ViewModels
{
    public class LeftPanelViewModel : BaseViewModel
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(LeftPanelViewModel));

        private readonly MeetingManagerService _meetingService;

        public ObservableCollection<Meeting> AllMeetings { get; }
        public ICollectionView MeetingsView { get; }

        private Meeting _selectedMeeting;
        public Meeting SelectedMeeting
        {
            get => _selectedMeeting;
            set
            {
                _selectedMeeting = value;
                OnPropertyChanged();
                UpdateCommands();

                _logger.Info($"SelectedMeeting changed: {SelectedMeeting?.Title ?? "None"}");
            }
        }

        private string _filterQuery;
        public string FilterQuery
        {
            get => _filterQuery;
            set
            {
                _filterQuery = value;
                OnPropertyChanged();
                MeetingsView.Refresh();

                _logger.Debug($"FilterQuery updated: '{_filterQuery}'");
            }
        }

        public ICommand RemoveMeetingCommand { get; }
        public ICommand AddMeetingCommand { get; }

        public Action<Meeting> OnAddMeeting { get; set; }

        public LeftPanelViewModel(MeetingManagerService meetingService)
        {
            _meetingService = meetingService ?? throw new ArgumentNullException(nameof(meetingService));

            AllMeetings = new ObservableCollection<Meeting>(
                _meetingService.GetAllMeetings()
            );

            _logger.Info($"Loaded {AllMeetings.Count} meetings from service");

            MeetingsView = CollectionViewSource.GetDefaultView(AllMeetings);
            MeetingsView.Filter = FilterMeetings;

            RemoveMeetingCommand = new RelayCommand(RemoveSelectedMeeting, () => SelectedMeeting != null);
            AddMeetingCommand = new RelayCommand(AddMeeting);
        }

        private bool FilterMeetings(object obj)
        {
            if (string.IsNullOrWhiteSpace(FilterQuery)) return true;
            if (obj is Meeting m)
            {
                bool match = m.Title?.IndexOf(FilterQuery, StringComparison.OrdinalIgnoreCase) >= 0
                             || m.Agenda?.IndexOf(FilterQuery, StringComparison.OrdinalIgnoreCase) >= 0
                             || m.Notes.Any(n => n.Content?.IndexOf(FilterQuery, StringComparison.OrdinalIgnoreCase) >= 0);

                if (match)
                    _logger.Debug($"Meeting '{m.Title}' matches filter '{FilterQuery}'");

                return match;
            }
            return false;
        }

        private void RemoveSelectedMeeting()
        {
            if (SelectedMeeting != null)
            {
                try
                {
                    _logger.Info($"Removed meeting: {SelectedMeeting.Title}");
                    _meetingService.RemoveMeeting(SelectedMeeting);
                    AllMeetings.Remove(SelectedMeeting);
                    SelectedMeeting = null;
                }
                catch (Exception ex)
                {
                    _logger.Error("Failed to remove meeting", ex);
                }
            }
        }

        private void AddMeeting()
        {
            _logger.Info("AddMeeting invoked");
            OnAddMeeting?.Invoke(null);
        }

        private void UpdateCommands()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
