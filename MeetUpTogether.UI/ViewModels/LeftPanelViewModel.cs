using MeetUpTogether.UI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace MeetUpTogether.UI.ViewModels
{
    public class LeftPanelViewModel : BaseViewModel
    {
        public ObservableCollection<Meeting> AllMeetings { get; } = new ObservableCollection<Meeting>();
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
        public Action<Meeting> OnMeetingRemoved { get; set; }

        public LeftPanelViewModel()
        {
            MeetingsView = CollectionViewSource.GetDefaultView(AllMeetings);
            MeetingsView.Filter = FilterMeetings;

            RemoveMeetingCommand = new RelayCommand(RemoveSelectedMeeting, () => SelectedMeeting != null);
            AddMeetingCommand = new RelayCommand(AddMeeting);
        }

        private bool FilterMeetings(object obj)
        {
            if (string.IsNullOrWhiteSpace(FilterQuery)) return true;
            if (obj is Meeting meeting)
            {
                return meeting.Title?.IndexOf(FilterQuery, StringComparison.OrdinalIgnoreCase) >= 0
                    || (meeting.Agenda?.IndexOf(FilterQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                    || meeting.Notes.Any(n => n.Content?.IndexOf(FilterQuery, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return false;
        }

        private void RemoveSelectedMeeting()
        {
            if (SelectedMeeting != null)
            {
                var removed = SelectedMeeting;
                AllMeetings.Remove(removed);
                OnMeetingRemoved?.Invoke(removed);
                SelectedMeeting = null;
            }
        }

        private void AddMeeting()
        {
            // Call a callback to create a new meeting in the RightPanel
            OnAddMeeting?.Invoke(null);
        }

        private void UpdateCommands()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
