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
        public ReportService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        private string GetReportsFolder()
        {
            var exeFolder = AppContext.BaseDirectory;
            var solutionRoot = Path.GetFullPath(Path.Combine(exeFolder, "..", "..", ".."));
            var reportsFolder = Path.Combine(solutionRoot, "reports");

            Directory.CreateDirectory(reportsFolder);
            return reportsFolder;
        }

        public string GenerateMeetingReport(IEnumerable<Meeting> meetings)
        {
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
            return filePath;
        }
    }
}