using MeetUpTogether.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUpTogether.DAL
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly AppDbContext _context;

        public MeetingRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Meeting> GetAll()
        {
            return _context.Meetings
                .Include(m => m.Notes)
                .ToList();
        }

        public void Add(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
            _context.SaveChanges();
        }

        public void Update(Meeting meeting)
        {
            _context.Meetings.Update(meeting);
            _context.SaveChanges();
        }

        public void Delete(Meeting meeting)
        {
            _context.Meetings.Remove(meeting);
            _context.SaveChanges();
        }
    }
}
