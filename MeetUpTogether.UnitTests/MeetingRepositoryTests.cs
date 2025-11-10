using MeetUpTogether.DAL;
using MeetUpTogether.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace MeetUpTogether.UnitTests
{
    [TestFixture]
    public class MeetingRepositoryTests
    {
        private AppDbContext _context;
        private MeetingRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            _context = new AppDbContext(options);
            _repository = new MeetingRepository(_context);
        }

        [Test]
        public void AddMeeting_Should_AddMeetingToDb()
        {
            var meeting = new Meeting { Title = "DAL Test Meeting" };

            _repository.Add(meeting);

            var allMeetings = _repository.GetAll().ToList();

            Assert.That(allMeetings.Count, Is.EqualTo(1));
            Assert.That(allMeetings[0].Title, Is.EqualTo("DAL Test Meeting"));
        }

        [Test]
        public void UpdateMeeting_Should_ChangeMeetingTitle()
        {
            var meeting = new Meeting { Title = "Original Title" };
            _repository.Add(meeting);

            // Modify the meeting
            meeting.Title = "Updated Title";
            _repository.Update(meeting);

            var updatedMeeting = _repository.GetAll().First();
            Assert.That(updatedMeeting.Title, Is.EqualTo("Updated Title"));
        }

        [Test]
        public void DeleteMeeting_Should_RemoveMeetingFromDb()
        {
            var meeting = new Meeting { Title = "To Be Deleted" };
            _repository.Add(meeting);

            _repository.Delete(meeting);

            var allMeetings = _repository.GetAll().ToList();
            Assert.That(allMeetings, Is.Empty);
        }
    }
}
