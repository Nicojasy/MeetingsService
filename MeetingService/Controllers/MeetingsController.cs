using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MeetingsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static MeetingsService.Controllers.Dto;
using MeetingsService.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace MeetingsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        ApplicationContext db;
        public MeetingsController(ApplicationContext context)
        {
            db = context;
        }

        //Invoke-RestMethod http://localhost:50590/api/meetings/viewallmeetings -Method POST
        // POST api/meetings
        [HttpPost]
        [Route("[action]")]
        public ActionResult ViewAllMeetings()
        {
            return new JsonResult(GetMeetingsJson1());
        }

        protected List<MeetingDtoView> GetMeetingsJson1()
        {
            List<MeetingDtoView> meetings = new List<MeetingDtoView>();
            foreach (var meeting in db.Meetings.Include(m => m.MeetingAttendees).ThenInclude(ma => ma.Attendee).ToList())
            {
                if (meeting.MeetingAttendees.Count != 0)
                {
                    List<AttendeeDto> attendeesDto = new List<AttendeeDto>();
                    foreach (var attendee in meeting.MeetingAttendees.Select(sc => sc.Attendee).ToList())
                    {
                        attendeesDto.Add(new AttendeeDto { name = attendee.Name, email = attendee.Email });

                    }
                    meetings.Add(new MeetingDtoView { title = meeting.Title, datetimestart = meeting.DatetimeStart.ToString(), datetimeend = meeting.DatetimeEnd.ToString(), attendeesDto = attendeesDto });
                }
                else
                {
                    meetings.Add(new MeetingDtoView { title = meeting.Title, datetimestart = meeting.DatetimeStart.ToString(), datetimeend = meeting.DatetimeEnd.ToString()});
                }
            }
            return meetings;
        }

        //Invoke-RestMethod http://localhost:50590/api/meetings/deleteattendee/2 -Method POST
        // DELETE api/meetings/deleteattendee/5
        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<ActionResult> DeleteAttendee(int id)
        {
            Attendee attendee = db.Attendees.FirstOrDefault(a => a.Id == id);
            if (attendee == null)
            {
                return NotFound("Attendee is null!");
            }
            db.Attendees.Remove(attendee);
            await db.SaveChangesAsync();
            return Ok($"{attendee.Name}: {attendee.Email} is deleted");
        }

        //Invoke-RestMethod http://localhost:50590/api/meetings/deletemeeting/2 -Method POST
        // DELETE api/meeting/deletemeeting/5
        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<ActionResult> DeleteMeeting(int id)
        {
            Meeting meeting = db.Meetings.FirstOrDefault(m => m.Id == id);
            if (meeting == null)
            {
                return NotFound("Meeting is null!");
            }
            db.Meetings.Remove(meeting);
            await db.SaveChangesAsync();
            return Ok($"{meeting.Title} is deleted");
        }

        //Invoke-RestMethod http://localhost:50590/api/meetings/AddMeeting -Method POST -Body (@{title = "Meeting6"; datetimestart = "05/10/2020 13:00"; datetimeend = "05/10/2020 17:00"} | ConvertTo-Json) -ContentType "application/json; charset=utf-8"
        // ADD api/meeting/addmeeting
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddMeeting(MeetingDto dto)
        {
            if (!Validation.IsValidName(dto.title))
                return BadRequest($"Error: {dto.title}");
            //sql: YYYY-MM-DD hh:mm:ss 1000-01-01 00:00:00 to 9999-12-31 23:59:59
            if (!DateTime.TryParseExact(dto.datetimestart, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime datetimeStart))
                return BadRequest($"{dto.datetimestart} is not in an acceptable format.");
            if (!DateTime.TryParseExact(dto.datetimeend, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime datetimeEnd))
                return BadRequest($"{dto.datetimeend} is not in an acceptable format.");
            if (datetimeStart > datetimeEnd)
                return BadRequest($"Error: {datetimeStart}>{datetimeEnd}");

            db.Meetings.Add(new Meeting { Title = dto.title, DatetimeStart = datetimeStart, DatetimeEnd = datetimeEnd});
            await db.SaveChangesAsync();
            return Ok($"Meeting \"{dto.title}\" added");
        }

        //Invoke-RestMethod http://localhost:50590/api/meetings/AddAttendee -Method POST -Body (@{meetingid = "3"; name = "Jack"; email="jack@gmail.com"} | ConvertTo-Json) -ContentType "application/json; charset=utf-8"
        // ADD api/meeting/addattendee
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddAttendee(AttendeesDto dto)
        {
            if (!int.TryParse(dto.meetingid, out int meetingid))
                return BadRequest($"Meeting \"{meetingid}\" must be a number!");
            if (!db.Meetings.Any(m => m.Id == meetingid))
                return BadRequest($"Meeting \"{meetingid}\" does not exist!)");

            string badResult = "";
            string goodResult = "";
            Meeting meeting = db.Meetings.First(m => m.Id == meetingid);
            List<Attendee> atList = new List<Attendee>();
            foreach (AttendeeDto atDto in dto.attendee)
            {
                if (!Validation.IsValidName(atDto.name))
                    badResult += $"\n Name \"{atDto.name}\" is incorrect!";
                else
                {
                    if (!Validation.IsValidEmail(atDto.email))
                        badResult += $"\n Email \"{atDto.email}\" is incorrect!";
                    else
                    {
                        Attendee attendee = db.Attendees.Where(a => a.Email == atDto.email).FirstOrDefault();
                        if (attendee != null)
                        {
                            if (attendee.MeetingAttendees.Any(ma => ma.MeetingId == meetingid))
                                badResult += $"\n Email \"{atDto.email}\" has already been added to the meeting!";
                            else
                            {
                                if (db.Meetings.Where(m => m.MeetingAttendees.Any(ma => ma.AttendeeId == attendee.Id))
                                               .Any(m => !(meeting.DatetimeEnd <= m.DatetimeStart ||
                                                    meeting.DatetimeStart >= m.DatetimeEnd)))
                                    badResult += $"\n {atDto.name}: {atDto.email} Time is busy!";
                                else
                                {
                                    if (attendee.Name == atDto.name)
                                    {
                                        attendee.MeetingAttendees.Add(new MeetingAttendee { MeetingId = meetingid, AttendeeId = attendee.Id });
                                        db.SaveChanges();
                                        goodResult += $"\n {atDto.name}: {atDto.email} Attendee added";
                                    }
                                    else
                                    {
                                        //this should not happen
                                        attendee.MeetingAttendees.Add(new MeetingAttendee { MeetingId = meetingid, AttendeeId = attendee.Id });
                                        db.SaveChanges();
                                        goodResult += $"\n {atDto.name} previously created as {attendee.Name}: {atDto.email} Attendee added";
                                    }
                                }
                            }
                        }
                        else
                        {
                            atList.Add(new Attendee { Name = atDto.name, Email = atDto.email });
                            db.Attendees.Add(new Attendee { Name = atDto.name, Email = atDto.email });
                            db.SaveChanges();
                            goodResult += $"\n {atDto.name}: {atDto.email} Attendee created and added";
                        }
                    }
                }
            }
            foreach (Attendee at in atList)
            {
                at.MeetingAttendees.Add(new MeetingAttendee { MeetingId = meetingid, AttendeeId = at.Id });
            }
            db.SaveChanges();

            string result = goodResult != "" ? $"Attendees has been added to the \"{db.Meetings.First(m => m.Id == meetingid).Title}\":{goodResult}\n" : "";
            result += badResult != "" ? $"Attendees has not been added:{badResult}" : "";
            
            // Token
            foreach (Attendee attendee in atList)
            {
                await Token(attendee.Id, attendee.Name, attendee.Email);
            }
            if (result != "")
                return Ok(result);
            else
                return BadRequest("Request is incorrect");
        }

        [HttpGet]
        [Route("[action]/{id}/{code}")]
        public ActionResult ConfirmEmail(string idString, string code)
        {
            if (idString == null || code == null)
                return BadRequest("Id and Code equal Null");
            if (!int.TryParse(idString,out int attendeeId))
                return BadRequest("Id is incorrect!");
            if (!db.Attendees.Any(a=>a.Id==attendeeId))
                return BadRequest("User is not found!");

            Token token = db.Tokens.FirstOrDefault(t=>t.AttendeeId== attendeeId && t.Code==code);
            if (token != null)
            {
                Attendee attendee = db.Attendees.FirstOrDefault(a => a.Id == attendeeId);
                if (attendee != null)
                {
                    attendee.IsEmailConfirmed = true;
                    return Ok("Thank you, your email is confirmed!");
                }
                else
                    return BadRequest("Sorry, there is no such user!");
            }
            else
                return BadRequest("Sorry, wrong data!");
        }

        [HttpPost]
        public async Task Token(int id, string name, string email)
        {

            var now = DateTime.UtcNow;
            // create JWT-token
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            db.Tokens.Add(new Token () { AttendeeId = id, Code = encodedJwt});

            //email
            var callbackUrl = Url.Action(
                "CoonfirmEmail",
                "Meetings",
                new { attendeId = id, code = encodedJwt},
                protocol: HttpContext.Request.Scheme);
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(email, "Confirm your account",
                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
        }
    }
}