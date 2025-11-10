using MeetUpTogether.BLL.Services;
using MeetUpTogether.Models;
using System;
using System.IO;
using System.Windows.Input;

namespace MeetUpTogether.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MeetingManagerService _meetingService;
        private readonly ReportService _reportService;
        public ICommand GenerateReportCommand { get; }

        public LeftPanelViewModel LeftPanel { get; }
        public RightPanelViewModel RightPanel { get; }
        public SearchBarViewModel SearchBar { get; }

        public MainViewModel()
        {
            _meetingService = ServiceFactory.CreateMeetingManager();

            _reportService = new ReportService();

            GenerateReportCommand = new RelayCommand(GenerateReport);

            LeftPanel = new LeftPanelViewModel(_meetingService);
            RightPanel = new RightPanelViewModel(_meetingService);
            SearchBar = new SearchBarViewModel();

            // Search filtering
            SearchBar.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchBar.SearchQuery))
                    LeftPanel.FilterQuery = SearchBar.SearchQuery;
            };

            // Add new meeting
            LeftPanel.OnAddMeeting = _ =>
            {
                RightPanel.CurrentMeeting = new Meeting
                {
                    From = DateTime.Today,
                    To = DateTime.Today
                };
            };

            // Save callback
            RightPanel.OnSave = meeting =>
            {
                if (!LeftPanel.AllMeetings.Contains(meeting))
                    LeftPanel.AllMeetings.Add(meeting);

                LeftPanel.MeetingsView.Refresh();
                LeftPanel.SelectedMeeting = null;
            };

            // When a meeting is selected
            LeftPanel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(LeftPanel.SelectedMeeting))
                {
                    RightPanel.CurrentMeeting = LeftPanel.SelectedMeeting;
                }
            };
        }
        public void GenerateReport()
        {
            try
            {
                var reportPath = _reportService.GenerateMeetingReport(LeftPanel.AllMeetings);
                System.Diagnostics.Debug.WriteLine($"Report generated successfully: {reportPath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to generate report: {ex.Message}");
            }
        }
    }
}
