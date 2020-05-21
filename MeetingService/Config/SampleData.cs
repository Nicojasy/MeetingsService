using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingsService.Models
{
    public static class SampleData
    {
        public static void Initialize(ApplicationContext context)
        {

            if (!(context.Attendees.Any() || context.Meetings.Any()))
            {
                Meeting m0 = new Meeting { Title = "Meeting1", DatetimeStart = new DateTime(2020, 06, 07, 16, 00, 00), DatetimeEnd = new DateTime(2020, 06, 07, 17, 00, 00), IsNewsletterDone = false};
                Meeting m1 = new Meeting { Title = "Meeting2", DatetimeStart = new DateTime(2020, 07, 07, 15, 30, 00), DatetimeEnd = new DateTime(2020, 07, 07, 17, 00, 00), IsNewsletterDone = false };
                Meeting m2 = new Meeting { Title = "Meeting3", DatetimeStart = new DateTime(2020, 05, 29, 12, 10, 00), DatetimeEnd = new DateTime(2020, 05, 29, 12, 30, 00), IsNewsletterDone = false };
                context.Meetings.AddRange(new List<Meeting> { m0, m1, m2 });
                Attendee a0 = new Attendee { Name = "Charley", Email = "Charley@gmail.com", IsEmailConfirmed = false};
                Attendee a1 = new Attendee { Name = "Alice", Email = "Henry@gmail.com", IsEmailConfirmed = false};
                Attendee a2 = new Attendee { Name = "Bob", Email = "Alice@gmail.com", IsEmailConfirmed = false};
                context.Attendees.AddRange(new List<Attendee> { a0, a1, a2 });
                context.SaveChanges();

                m0.MeetingAttendees.Add(new MeetingAttendee { MeetingId = m0.Id, AttendeeId = a0.Id });
                m0.MeetingAttendees.Add(new MeetingAttendee { MeetingId = m0.Id, AttendeeId = a1.Id });
                m1.MeetingAttendees.Add(new MeetingAttendee { MeetingId = m1.Id, AttendeeId = a1.Id });
                m1.MeetingAttendees.Add(new MeetingAttendee { MeetingId = m1.Id, AttendeeId = a2.Id });
                m2.MeetingAttendees.Add(new MeetingAttendee { MeetingId = m2.Id, AttendeeId = a2.Id });
                context.SaveChanges();
            }
        }
    }
}