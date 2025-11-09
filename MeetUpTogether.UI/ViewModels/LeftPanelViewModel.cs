using MeetUpTogether.Models;
using MeetUpTogether.BLL.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace MeetUpTogether.UI.ViewModels
{
    public class LeftPanelViewModel : BaseViewModel
    {
        private readonly MeetingManagerService _meetingService;

        public ObservableCollection<Meeting> AllMeetings { get; }
        public ICollectionView MeetingsView { get; }

        private Meeting _selectedMeeting;
        public Meeting SelectedMeeting
        {
            get => _selectedMeeting;
            set { _selectedMeeting = value; OnPropertyChanged(); UpdateCommands(); }
        }

        private string _filterQuery;
        public string FilterQuery
        {
            get => _filterQuery;
            set { _filterQuery = value; OnPropertyChanged(); MeetingsView.Refresh(); }
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
                return m.Title?.IndexOf(FilterQuery, StringComparison.OrdinalIgnoreCase) >= 0
                       || m.Agenda?.IndexOf(FilterQuery, StringComparison.OrdinalIgnoreCase) >= 0
                       || m.Notes.Any(n => n.Content?.IndexOf(FilterQuery, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return false;
        }

        private void RemoveSelectedMeeting()
        {
            if (SelectedMeeting != null)
            {
                _meetingService.RemoveMeeting(SelectedMeeting);
                AllMeetings.Remove(SelectedMeeting);
                SelectedMeeting = null;
            }
        }

        private void AddMeeting()
        {
            OnAddMeeting?.Invoke(null);
        }

        private void UpdateCommands()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}