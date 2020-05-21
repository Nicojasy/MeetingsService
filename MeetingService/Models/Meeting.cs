using System;
using System.Collections.Generic;

namespace MeetingsService.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DatetimeStart { get; set; }
        public DateTime DatetimeEnd { get; set; }
        public bool IsNewsletterDone { get; set; }
        public List<MeetingAttendee> MeetingAttendees { get; set; }

        public Meeting()
        {
            IsNewsletterDone = false;
            MeetingAttendees = new List<MeetingAttendee>();
        }
    }
}
