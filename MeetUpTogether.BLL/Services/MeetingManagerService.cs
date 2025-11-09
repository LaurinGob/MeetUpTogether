using MeetUpTogether.DAL;
using MeetUpTogether.Models;
using System.Collections.Generic;

namespace MeetUpTogether.BLL.Services
{
    public class MeetingManagerService
    {
        private readonly IMeetingRepository _repository;

        public MeetingManagerService(IMeetingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<Meeting> GetAllMeetings() => _repository.GetAll();

        public void AddMeeting(Meeting meeting) => _repository.Add(meeting);

        public void UpdateMeeting(Meeting meeting) => _repository.Update(meeting);

        public void RemoveMeeting(Meeting meeting) => _repository.Delete(meeting);
    }
}