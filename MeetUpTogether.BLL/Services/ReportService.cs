using MeetUpTogether.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;

namespace MeetUpTogether.BLL.Services
{
    public class ReportService
    {
        private readonly AppSettings _settings;
        private readonly LoggingService _logger = new();

        public ReportService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _settings = AppSettings.Load();
        }

        private string GetReportsFolder()
        {
            var reportsFolder = _settings.Paths.ReportsFolder;

            if (!Path.IsPathRooted(reportsFolder))
            {
                var exeFolder = AppContext.BaseDirectory;
                reportsFolder = Path.GetFullPath(Path.Combine(exeFolder, reportsFolder));
            }

            Directory.CreateDirectory(reportsFolder);
            return reportsFolder;
        }

        public string GenerateMeetingReport(IEnumerable<Meeting> meetings)
        {
            try
            {
                _logger.Info("Generating PDF report...");
                if (meetings == null) throw new ArgumentNullException(nameof(meetings));

                var reportsFolder = GetReportsFolder();
                var fileName = $"MeetUpReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                var filePath = Path.Combine(reportsFolder, fileName);

                var doc = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.Size(PageSizes.A4);

                        page.Header()
                            .Text("MeetUp Report")
                            .FontSize(20)
                            .Bold()
                            .AlignCenter();

                        page.Content()
                            .Column(column =>
                            {
                                foreach (var meeting in meetings)
                                {
                                    column.Item().Text($"Title: {meeting.Title}").Bold();
                                    column.Item().Text($"From: {meeting.From:d}  To: {meeting.To:d}");
                                    column.Item().Text($"Agenda: {meeting.Agenda}");

                                    if (meeting.Notes.Count > 0)
                                    {
                                        column.Item().Text("Notes:").Underline();
                                        foreach (var note in meeting.Notes)
                                        {
                                            column.Item().Text($"- {note.Content} ({note.CreatedAt:g})");
                                        }
                                    }

                                    column.Item().PaddingVertical(5).LineHorizontal(1);
                                }
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(text =>
                            {
                                text.CurrentPageNumber();
                                text.Span(" / ");
                                text.TotalPages();
                            });
                    });
                });

                doc.GeneratePdf(filePath);
                _logger.Info("Report successfully generated.");
                return filePath;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to generate report", ex);
                throw;
            }
        }
    }
}