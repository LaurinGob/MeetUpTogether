using MeetUpTogether.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetUpTogether.BLL.Services
{
    public class MeetingManagerService
    {
        private readonly List<Meeting> _meetings = new();

        public IEnumerable<Meeting> GetAll() => _meetings;

        public IEnumerable<Meeting> GetAllMeetings()
        {
            // Return a copy to avoid accidental external modification
            return _meetings.ToList();
        }

        public void AddMeeting(Meeting meeting)
        {
            if (meeting == null)
                throw new ArgumentNullException(nameof(meeting));

            if (string.IsNullOrWhiteSpace(meeting.Title))
                throw new ArgumentException("Meeting must have a title.");

            // Prevent duplicates
            if (!_meetings.Contains(meeting))
                _meetings.Add(meeting);
        }

        public void UpdateMeeting(Meeting meeting)
        {
            if (meeting == null)
                throw new ArgumentNullException(nameof(meeting));

            // Find by reference or by unique title if needed
            var existing = _meetings.FirstOrDefault(m => m == meeting);
            if (existing == null)
            {
                // Optionally add if not found
                _meetings.Add(meeting);
            }
            else
            {
                // Since Meeting is a class (reference type), this may not be needed
                // But if you later replace with persistence, this is where DB updates go
            }
        }

        public void RemoveMeeting(Meeting meeting)
        {
            if (meeting == null)
                return;

            if (_meetings.Contains(meeting))
                _meetings.Remove(meeting);
        }

        public void ClearAll()
        {
            _meetings.Clear();
        }
    }
}
