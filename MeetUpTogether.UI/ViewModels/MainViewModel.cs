using MeetUpTogether.BLL.Services;
using MeetUpTogether.Models;
using System.Windows.Input;

namespace MeetUpTogether.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MeetingManagerService _meetingService;
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

            _meetingService = new MeetingManagerService();

            LeftPanel = new LeftPanelViewModel(_meetingService);
            RightPanel = new RightPanelViewModel(_meetingService);

            SearchBar.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchBar.SearchQuery))
                    LeftPanel.FilterQuery = SearchBar.SearchQuery;
            };

            LeftPanel.OnAddMeeting = _ =>
            {
                RightPanel.CurrentMeeting = new Meeting
                {
                    From = DateTime.Today,
                    To = DateTime.Today
                };
                RightPanel.IsEditable = true;
            };

            RightPanel.OnSave = meeting =>
            {
                // Save the meeting via the BLL service
                _meetingService.AddMeeting(meeting);

                // Refresh UI list
                LeftPanel.RefreshMeetings();

                // Clear selection and disable editing
                LeftPanel.SelectedMeeting = null;
                RightPanel.CurrentMeeting = null;
                RightPanel.IsEditable = false;
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
                    else
                    {
                        RightPanel.CurrentMeeting = null;
                        RightPanel.IsEditable = false;
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
