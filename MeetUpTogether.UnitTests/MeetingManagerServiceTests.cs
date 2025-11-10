using MeetUpTogether.BLL.Services;
using MeetUpTogether.DAL;
using MeetUpTogether.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;

namespace MeetUpTogether.UnitTests
{
    [TestFixture]
    public class MeetingManagerServiceTests
    {
        private MeetingManagerService _service;
        private AppDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            // Configure EF Core to use in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new AppDbContext(options);
            _dbContext.Database.EnsureDeleted(); // Ensure clean DB for each test
            _dbContext.Database.EnsureCreated();

            var repository = new MeetingRepository(_dbContext);
            _service = new MeetingManagerService(repository);
        }

        [Test]
        public void AddMeeting_Should_AddMeetingToRepository()
        {
            var meeting = new Meeting { Title = "Test Meeting" };

            _service.AddMeeting(meeting);

            var allMeetings = _service.GetAllMeetings().ToList();
            Assert.That(allMeetings.Count, Is.EqualTo(1));
            Assert.That(allMeetings[0].Title, Is.EqualTo("Test Meeting"));
        }

        [Test]
        public void RemoveMeeting_Should_RemoveMeetingFromRepository()
        {
            var meeting = new Meeting { Title = "Remove Me" };
            _service.AddMeeting(meeting);

            _service.RemoveMeeting(meeting);

            var allMeetings = _service.GetAllMeetings().ToList();
            Assert.That(allMeetings, Is.Empty);
        }

        [Test]
        public void UpdateMeeting_Should_ChangeMeetingTitle()
        {
            var meeting = new Meeting { Title = "Old Title" };
            _service.AddMeeting(meeting);

            // Update the title
            meeting.Title = "New Title";
            _service.UpdateMeeting(meeting);

            var allMeetings = _service.GetAllMeetings().ToList();
            Assert.That(allMeetings.Count, Is.EqualTo(1));
            Assert.That(allMeetings[0].Title, Is.EqualTo("New Title"));
        }
    }
}
