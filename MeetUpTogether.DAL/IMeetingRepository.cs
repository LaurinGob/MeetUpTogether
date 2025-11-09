using MeetUpTogether.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUpTogether.DAL
{
    public interface IMeetingRepository
    {
        IEnumerable<Meeting> GetAll();
        void Add(Meeting meeting);
        void Update(Meeting meeting);
        void Delete(Meeting meeting);
    }
}
