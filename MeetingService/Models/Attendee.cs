using System.Collections.Generic;

namespace MeetingsService.Models
{
    public class Attendee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public List<MeetingAttendee> MeetingAttendees { get; set; }
        public Attendee()
        {
            IsEmailConfirmed = false;
            MeetingAttendees = new List<MeetingAttendee>();
        }
    }
}
