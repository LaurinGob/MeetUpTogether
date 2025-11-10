using MeetUpTogether.BLL.Services;
using MeetUpTogether.DAL;
using MeetUpTogether.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;

namespace MeetUpTogether.UnitTests
{
    [TestFixture]
    public class ServiceFactoryTests
    {
        private MeetingManagerService _service;

        [SetUp]
        public void Setup()
        {
            // Create a unique in-memory DbContext per test
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            var context = new AppDbContext(options);
            var repository = new MeetingRepository(context);

            _service = new MeetingManagerService(repository);
        }

        [Test]
        public void CreatedService_Should_BeNotNull()
        {
            Assert.That(_service, Is.Not.Null);
        }

        [Test]
        public void AddMeeting_Should_AddAndRetrieveMeeting()
        {
            var meeting = new Meeting { Title = "Factory Test" };
            _service.AddMeeting(meeting);

            var allMeetings = _service.GetAllMeetings().ToList();
            Assert.That(allMeetings.Count, Is.EqualTo(1));
            Assert.That(allMeetings[0].Title, Is.EqualTo("Factory Test"));
        }

        [Test]
        public void RemoveMeeting_Should_RemoveMeeting()
        {
            var meeting = new Meeting { Title = "Remove Test" };
            _service.AddMeeting(meeting);

            _service.RemoveMeeting(meeting);

            var allMeetings = _service.GetAllMeetings().ToList();
            Assert.That(allMeetings, Is.Empty);
        }

        [Test]
        public void UpdateMeeting_Should_UpdateExistingMeeting()
        {
            // Arrange: add a meeting
            var meeting = new Meeting { Title = "Old Title" };
            _service.AddMeeting(meeting);

            // Act: update the meeting
            meeting.Title = "New Title";
            _service.UpdateMeeting(meeting);

            // Assert
            var allMeetings = _service.GetAllMeetings().ToList();
            Assert.That(allMeetings.Count, Is.EqualTo(1));
            Assert.That(allMeetings[0].Title, Is.EqualTo("New Title"));
        }
    }
}
