using Microsoft.EntityFrameworkCore;

namespace MeetingsService.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeetingAttendee>()
                .HasKey(t => new { t.MeetingId, t.AttendeeId });

            modelBuilder.Entity<MeetingAttendee>()
                .HasOne(sc => sc.Meeting)
                .WithMany(s => s.MeetingAttendees)
                .HasForeignKey(sc => sc.MeetingId);

            modelBuilder.Entity<MeetingAttendee>()
                .HasOne(sc => sc.Attendee)
                .WithMany(c => c.MeetingAttendees)
                .HasForeignKey(sc => sc.AttendeeId);
        }
    }
}