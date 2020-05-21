using MeetingsService.Controllers;
using MeetingsService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static MeetingsService.Controllers.Dto;

namespace MeetingsService.Test
{
    public class UnitTestsAddMeeting
    {
        private List<Meeting> GetTestMeetings()
        {
            var users = new List<Meeting>
            {
            new Meeting { Id=1, Title="Meeting0", DatetimeStart = new DateTime(2020, 06, 07, 16, 00, 00), DatetimeEnd = new DateTime(2020, 06, 07, 17, 00, 00), IsNewsletterDone = false },
            new Meeting { Id=2, Title="Meeting1", DatetimeStart = new DateTime(2020, 06, 07, 16, 00, 00), DatetimeEnd = new DateTime(2020, 06, 07, 17, 00, 00), IsNewsletterDone = false },
            new Meeting { Id=3, Title="Meeting2", DatetimeStart = new DateTime(2020, 06, 07, 16, 00, 00), DatetimeEnd = new DateTime(2020, 06, 07, 17, 00, 00), IsNewsletterDone = false },
            new Meeting { Id=4, Title="Meeting3", DatetimeStart = new DateTime(2020, 06, 07, 16, 00, 00), DatetimeEnd = new DateTime(2020, 06, 07, 17, 00, 00), IsNewsletterDone = false },
                };
            return users;
        }
        [Theory]
        [InlineData("MeetingTestttttttttttttttttttttttttttttttttttttttttttttttttt","05/09/2020 20:42","05/09/2020 20:52")]
        [InlineData(null, "07/09/2020 20:42", "07/09/2020 20:52")]
        public async Task AddMeetingReturnsBadRequestResultWhenIsValidNameBad(string _title, string _datetimeStart, string _datetimeEnd)
        {

            var controller = new MeetingsController(null);
            
            var result = await controller.AddMeeting(new MeetingDto { title = _title, datetimestart = _datetimeStart, datetimeend = _datetimeEnd }) as BadRequestObjectResult;

            Assert.Equal(result.Value, $"Error: {_title}");
        }

        private IDisposable GetContextWithData()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddMeetingReturnsBadRequestResultWhenTryParseExactDatetimeStartIsBad()
        {

        }

        [Fact]
        public void AddMeetingReturnsBadRequestResultWhenTryParseExactDatetimeEndIsBad()
        {

        }

        [Fact]
        public void AddMeetingReturnsViewResultWithUserModel()
        {

        }
    }
}
