using MeetUpTogether.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUpTogether.BLL.Services
{
    public static class ServiceFactory
    {
        public static MeetingManagerService CreateMeetingManager()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=meetings.db")
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureCreated();

            var repository = new MeetingRepository(dbContext);

            return new MeetingManagerService(repository);
        }
    }
}
