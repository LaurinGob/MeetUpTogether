using MeetUpTogether.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MeetUpTogether.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Note> Notes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meeting>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Note>()
                .HasKey(n => n.Id);

            modelBuilder.Entity<Note>()
                .HasOne(n => n.Meeting)
                .WithMany(m => m.Notes)
                .HasForeignKey(n => n.MeetingId);
        }
    }
}
