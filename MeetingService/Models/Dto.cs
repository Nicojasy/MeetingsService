using System.Collections.Generic;

namespace MeetingsService.Controllers
{
    public class Dto
    {
        public class MeetingDto
        {
            public string title { get; set; }
            public string datetimestart { get; set; }
            public string datetimeend { get; set; }
        }
        public class AttendeesDto
        {
            public string meetingid { get; set; }
            public List<AttendeeDto> attendee { get; set; }
        }

        public class AttendeeDto
        {
            public string name { get; set; }
            public string email { get; set; }
        }
        public class MeetingDtoView
        {
            public string title { get; set; }
            public string datetimestart { get; set; }
            public string datetimeend { get; set; }
            public List<AttendeeDto> attendeesDto { get; set; }
        }
    }
}
