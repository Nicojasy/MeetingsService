namespace MeetingsService.Models
{
    public class MeetingAttendee
    {
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }

        public int AttendeeId { get; set; }
        public Attendee Attendee { get; set; }
    }
}
