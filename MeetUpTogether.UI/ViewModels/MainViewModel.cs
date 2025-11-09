using MeetUpTogether.UI.Models;
using System.Windows.Input;

namespace MeetUpTogether.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public LeftPanelViewModel LeftPanel { get; }
        public RightPanelViewModel RightPanel { get; }
        public SearchBarViewModel SearchBar { get; }
        public ICommand GenerateReportCommand { get; }

        public RelayCommand AddMeetingCommand { get; }

        private Meeting _currentMeeting;
        public Meeting CurrentMeeting
        {
            get => _currentMeeting;
            set
            {
                _currentMeeting = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            GenerateReportCommand = new RelayCommand(GenerateReport);
            SearchBar = new SearchBarViewModel();
            LeftPanel = new LeftPanelViewModel();
            RightPanel = new RightPanelViewModel();

            RightPanel.OnSave = meeting =>
            {
                if (!LeftPanel.AllMeetings.Contains(meeting))
                {
                    LeftPanel.AllMeetings.Add(meeting);
                }

                LeftPanel.SelectedMeeting = null;
                LeftPanel.MeetingsView.Refresh();
            };

            SearchBar.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchBar.SearchQuery))
                    LeftPanel.FilterQuery = SearchBar.SearchQuery;
            };

            LeftPanel.OnAddMeeting = _ =>
            {
                RightPanel.CurrentMeeting = new Meeting();
                RightPanel.IsEditable = true;
            };

            LeftPanel.OnMeetingRemoved = removedMeeting =>
            {
                if (RightPanel.CurrentMeeting == removedMeeting)
                {
                    RightPanel.CurrentMeeting = null;
                    RightPanel.NewNoteText = string.Empty;
                    RightPanel.IsEditable = false;
                }
            };

            LeftPanel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(LeftPanel.SelectedMeeting))
                {
                    if (LeftPanel.SelectedMeeting != null)
                    {
                        RightPanel.CurrentMeeting = LeftPanel.SelectedMeeting;
                        RightPanel.IsEditable = true;
                    }
                }
            };
        }
        private void GenerateReport()
        {
            // TODO: implement PDF generation logic here using an external library
            System.Diagnostics.Debug.WriteLine("GenerateReportCommand triggered");
        }
    }
}
