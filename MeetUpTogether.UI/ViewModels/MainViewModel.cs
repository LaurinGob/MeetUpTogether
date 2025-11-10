using MeetUpTogether.BLL.Services;
using MeetUpTogether.Models;
using System;
using System.IO;
using System.Windows.Input;
using log4net;

namespace MeetUpTogether.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(MainViewModel));

        private readonly MeetingManagerService _meetingService;
        private readonly ReportService _reportService;

        public ICommand GenerateReportCommand { get; }

        public LeftPanelViewModel LeftPanel { get; }
        public RightPanelViewModel RightPanel { get; }
        public SearchBarViewModel SearchBar { get; }

        public MainViewModel()
        {
            _logger.Info("Initializing MainViewModel");

            _meetingService = ServiceFactory.CreateMeetingManager();
            _reportService = new ReportService();

            GenerateReportCommand = new RelayCommand(GenerateReport);

            LeftPanel = new LeftPanelViewModel(_meetingService);
            RightPanel = new RightPanelViewModel(_meetingService);
            SearchBar = new SearchBarViewModel();

            _logger.Info($"Loaded {LeftPanel.AllMeetings.Count} meetings from service");

            // Search filtering
            SearchBar.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchBar.SearchQuery))
                {
                    LeftPanel.FilterQuery = SearchBar.SearchQuery;
                    _logger.Debug($"Search query updated: '{SearchBar.SearchQuery}'");
                }
            };

            // Add new meeting
            LeftPanel.OnAddMeeting = _ =>
            {
                RightPanel.CurrentMeeting = new Meeting
                {
                    From = DateTime.Today,
                    To = DateTime.Today
                };
                _logger.Info("New meeting created in RightPanel");
            };

            // Save callback
            RightPanel.OnSave = meeting =>
            {
                if (!LeftPanel.AllMeetings.Contains(meeting))
                {
                    LeftPanel.AllMeetings.Add(meeting);
                    _logger.Info($"Meeting saved and added: {meeting.Title}");
                }
                else
                {
                    _logger.Info($"Meeting updated: {meeting.Title}");
                }

                LeftPanel.MeetingsView.Refresh();
                LeftPanel.SelectedMeeting = null;
            };

            // When a meeting is selected
            LeftPanel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(LeftPanel.SelectedMeeting))
                {
                    RightPanel.CurrentMeeting = LeftPanel.SelectedMeeting;
                    _logger.Info($"Selected meeting changed: {LeftPanel.SelectedMeeting?.Title ?? "None"}");
                }
            };
        }

        public void GenerateReport()
        {
            try
            {
                var reportPath = _reportService.GenerateMeetingReport(LeftPanel.AllMeetings);
                _logger.Info($"Report generated successfully: {reportPath}");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to generate report", ex);
            }
        }
    }
}
